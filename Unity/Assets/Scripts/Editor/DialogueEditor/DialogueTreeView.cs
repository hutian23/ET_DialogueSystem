using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public class DialogueTreeView: GraphView
    {
        private readonly Vector2 DefaultNodeSize = new(200, 150);
        private readonly Vector2 DefaultCommentSize = new(300, 200);

        public new class UxmlFactory: UxmlFactory<DialogueTreeView, UxmlTraits>
        {
        }

        private DialogueTree tree;
        private DialogueEditor window;
        private SearchMenuWindowProvider searchWindow;

        private List<DialogueNode> removeCaches = new();
        private List<CommentBlockData> removeBlockCaches = new();

        public DialogueTreeView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContextualMenuManipulator(this.OnContextMenuPopulate));

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uss");
            styleSheets.Add(styleSheet);
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    switch (elem)
                    {
                        case DialogueNodeView nodeView:
                            this.removeCaches.Add(nodeView.node);
                            break;
                        case CommentBlockGroup group:
                            this.removeBlockCaches.Add(group.blockData);
                            break;
                    }
                });
            }

            SetDirty();
            return graphViewChange;
        }

        public void PopulateView(DialogueTree _tree, DialogueEditor dialogueEditor)
        {
            tree = _tree;
            window = dialogueEditor;

            removeCaches.Clear();
            removeBlockCaches.Clear();

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            //1. 搜索框
            AddSearchWindow();
            //2. 生成视图节点
            //如果没有根节点，则创建
            if (tree.root == null)
            {
                RootNode rootNode = tree.CreateRoot();
                tree.root = rootNode;
                rootNode.position = new Vector2(100, 200);
            }
            //注意!!! 深拷贝之后，如果rootNode在nodes中，则会有两个rootNode
            //这里rootNode的视图和连线要额外处理
            DialogueNodeView rootView = CreateNodeView(tree.root);
            foreach (var node in tree.nodes)
            {
                CreateNodeView(node);
            }
            //3. 生成边
            rootView.GenerateEdge();
            foreach (var node in tree.nodes)
            {
                DialogueNodeView nodeView = GetViewFromNode(node);
                nodeView.GenerateEdge();
            }

            //4. 生成背景板
            foreach (var block in tree.blockDatas)
            {
                CreateCommentBlockView(block);
            }

            RegisterCallback<KeyDownEvent>(KeyDownEventCallback);
            RegisterCallback<MouseEnterEvent>(MouseEnterControl);
        }

        private void AddSearchWindow()
        {
            searchWindow = ScriptableObject.CreateInstance<SearchMenuWindowProvider>();
            searchWindow.Init(window, this);
            //添加回调，按下空格调用
            nodeCreationRequest = context =>
            {
                //打开一个searchWindow
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
            };
        }

        private void OnContextMenuPopulate(ContextualMenuPopulateEvent evt)
        {
            if (selection.Count == 1 && selection[0] is DialogueNodeView)
            {
                switch (selection[0])
                {
                    case DialogueNodeView nodeView:
                        evt.menu.AppendAction("Remove from commentBlock", _ => { RemoveNodeFromGroup(nodeView); });
                        break;
                }
            }

            evt.menu.AppendAction("Redo", _ => this.OnRedo());
        }

        private void RemoveNodeFromGroup(DialogueNodeView nodeView)
        {
            foreach (var group in tree.blockDatas)
            {
                group.children.Remove(nodeView.viewDataKey);
            }

            this.PopulateView(tree, window);
        }

        public override List<Port> GetCompatiblePorts(Port startPorts, NodeAdapter nodeAdapter)
        {
            var list = ports.ToList().Where(endPort =>
                    endPort.direction != startPorts.direction &&
                    endPort.direction != startPorts.direction &&
                    endPort.node != startPorts.node).ToList();
            return list;
        }

        private void MouseEnterControl(MouseEnterEvent evt)
        {
            var nodeList = this.selection.OfType<DialogueNodeView>().Select(nodeView => nodeView.node).ToList();
            this.window.inspectorView.UpdateSelection(nodeList);
        }

        private void KeyDownEventCallback(KeyDownEvent evt)
        {
            if (!evt.ctrlKey) return;
            switch (evt.keyCode)
            {
                //自动保存
                case KeyCode.A:
                    window.autoSaveToggle.value = !window.autoSaveToggle.value;
                    evt.StopPropagation();
                    break;
                //手动保存
                case KeyCode.S:
                    SaveDialogueTree();
                    evt.StopPropagation();
                    break;
                //撤销
                case KeyCode.Z:
                    OnRedo();
                    evt.StopPropagation();
                    break;
                //复制
                case KeyCode.C:
                    Copy();
                    evt.StopPropagation();
                    break;
                case KeyCode.V:
                    Duplicate();
                    evt.StopPropagation();
                    break;
            }
        }

        private void OnRedo()
        {
            SetDirty(false);
            PopulateView(tree, window);
        }

        //标记一下视图中有修改
        public void SetDirty(bool HasUnSave = true)
        {
            window.HasUnSave = HasUnSave;
        }

        public void SaveDialogueTree()
        {
            for (int i = 0; i < removeCaches.Count; i++)
            {
                tree.DeleteNode(removeCaches[i]);
            }

            this.removeCaches.Clear();
            for (int i = 0; i < removeBlockCaches.Count; i++)
            {
                tree.DeleteBlock(removeBlockCaches[i]);
            }

            this.removeBlockCaches.Clear();
            SaveCommentBlock();
            SaveNodes();
            window.HasUnSave = false;
            EditorUtility.SetDirty(tree);
            //刷新页面
            PopulateView(tree, window);
        }

        private void Copy()
        {
            switch (selection.FirstOrDefault())
            {
                case DialogueNodeView nodeView:
                    if (nodeView.node is RootNode) break;
                    DialogueSettings.GetSettings().copyNode = nodeView.Clone();
                    break;
            }
        }

        private void Duplicate()
        {
            switch (DialogueSettings.GetSettings().copyNode)
            {
                case DialogueNode copyNode:
                    DialogueNode dialogueNode = MongoHelper.Clone(copyNode);
                    dialogueNode.Guid = GUID.Generate().ToString();
                    dialogueNode.position = copyNode.position;
                    dialogueNode.TargetID = 0;
                    CreateNode(dialogueNode);
                    break;
            }
        }

        public void RefreshNodeState()
        {
            graphElements.OfType<DialogueNodeView>().ToList().ForEach(view =>
            {
                Color color;
                DialogueSettings settings = DialogueSettings.GetSettings();
                switch (view.node.Status)
                {
                    case Status.Pending:
                        color = settings.PendingColor;
                        break;
                    case Status.Failed:
                        color = settings.FailedColor;
                        break;
                    case Status.Success:
                        color = settings.SuccessColor;
                        break;
                    default:
                        color = settings.DefaultColor;
                        break;
                }

                view.titleContainer.style.backgroundColor = color;
            });
        }

        #region Node

        /// <summary>
        /// nodeview的viewDataKey = node.Guid
        /// </summary>
        private DialogueNodeView GetViewFromNode(DialogueNode node)
        {
            if (node == null) return null;
            return GetNodeByGuid(node.Guid) as DialogueNodeView;
        }

        public void CreateNode(Type type, Vector2 position)
        {
            DialogueNode node = tree.CreateDialogueNode(type);
            node.position = position;
            CreateNodeView(node);
        }

        private void CreateNode(DialogueNode node)
        {
            tree.nodes.Add(node);
            CreateNodeView(node);
        }

        private DialogueNodeView CreateNodeView(DialogueNode node)
        {
            Assembly assembly = typeof (DialogueNodeView).Assembly;
            //dialogueNodeView的子类
            List<Type> ret = assembly.GetTypes().Where(type => type.IsClass && type.IsSubclassOf(typeof (DialogueNodeView))).ToList();
            foreach (var nodeViewType in ret)
            {
                NodeEditorOfAttribute attr = nodeViewType.GetCustomAttribute(typeof (NodeEditorOfAttribute)) as NodeEditorOfAttribute;
                if (attr.nodeType == node.GetType())
                {
                    DialogueNodeView nodeView = Activator.CreateInstance(nodeViewType, args: new object[] { node, this }) as DialogueNodeView;
                    nodeView.SetPosition(new Rect(node.position, DefaultNodeSize));
                    AddElement(nodeView);
                    return nodeView;
                }
            }
            return null;
        }

        private void SaveNodes()
        {
            //从根节点开始遍历，字典存储在树上的节点(不在树上的节点保存nodeList中)
            HashSet<DialogueNodeView> nodeSet = new(); // 被遍历过的节点
            int IdGenerator = 0;

            Queue<DialogueNodeView> workQueue = new();

            DialogueNodeView rootView = GetViewFromNode(tree.root);
            tree.targets.Clear();
            workQueue.Enqueue(rootView);
            nodeSet.Add(rootView);
            while (workQueue.Count != 0)
            {
                DialogueNodeView nodeView = workQueue.Dequeue();

                nodeView.node.TargetID = IdGenerator;
                tree.targets.TryAdd(IdGenerator, nodeView.node);
                IdGenerator++;

                foreach (var output in nodeView.outports)
                {
                    foreach (var edge in output.connections)
                    {
                        DialogueNodeView childView = edge.input.node as DialogueNodeView;
                        if (nodeSet.Contains(childView)) continue;
                        workQueue.Enqueue(childView);
                        nodeSet.Add(childView);
                    }
                }
            }

            List<DialogueNodeView> nodeViews = graphElements.Where(x => x is DialogueNodeView).Cast<DialogueNodeView>().ToList();
            nodeViews.ForEach(view => view.SaveCallback?.Invoke());
        }

        #endregion

        #region CommentBlock

        public void CreateCommentBlock(Vector2 position)
        {
            CommentBlockData blockData = tree.CreateBlock(position);
            CreateCommentBlockView(blockData);
        }

        private void CreateCommentBlockView(CommentBlockData blockData)
        {
            var group = new CommentBlockGroup(blockData, this);
            group.SetPosition(new Rect(blockData.position, DefaultCommentSize));
            AddElement(group);
            foreach (var guid in group.blockData.children)
            {
                DialogueNodeView nodeView = GetNodeByGuid(guid) as DialogueNodeView;
                if (nodeView == null) continue;
                group.AddElement(nodeView);
            }
        }

        private void SaveCommentBlock()
        {
            List<CommentBlockGroup> groups = graphElements.Where(x => x is CommentBlockGroup).Cast<CommentBlockGroup>().ToList();
            groups.ForEach(block => block.Save());
            EditorUtility.SetDirty(tree);
        }

        #endregion

        #region Edge

        /// <summary>
        /// 只有父节点的output和子节点的input的连接逻辑需要重载
        /// </summary>
        public Edge CreateEdge(Port output, int targetID)
        {
            if (output == null || output.direction == Direction.Input) return null;
            tree.targets.TryGetValue(targetID, out DialogueNode node);
            DialogueNodeView nodeView = GetViewFromNode(node);
            //RootNode没有input
            if (nodeView == null || node is RootNode) return null;

            Edge edge = output.ConnectTo(nodeView.input);
            AddElement(edge);
            return edge;
        }

        public void CreateEdges(Port output, List<int> targets)
        {
            if (output == null || output.direction == Direction.Input || targets == null) return;
            foreach (var targetID in targets)
            {
                tree.targets.TryGetValue(targetID, out DialogueNode node);
                DialogueNodeView nodeView = GetViewFromNode(node);
                if (nodeView == null || node is RootNode) continue;
                Edge edge = output.ConnectTo(nodeView.input);
                AddElement(edge);
            }
        }

        #endregion
    }
}