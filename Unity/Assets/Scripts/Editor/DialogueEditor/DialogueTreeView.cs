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

        private readonly List<object> RemoveCaches = new();
        
        public DialogueTreeView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContextualMenuManipulator(this.OnContextMenuPopulate));

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/Resource/DialogueEditor.uss");
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
                            RemoveCaches.Add(nodeView.node);
                            break;
                        case CommentBlockGroup group:
                            RemoveCaches.Add(group.blockData);
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
            
            RemoveCaches.Clear();
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            //1. 生成搜索框
            AddSearchWindow();
            //2. 生成视图节点
            //如果没有根节点，则创建
            if (tree.root == null)
            {
                RootNode rootNode = tree.CreateRoot();
                tree.root = rootNode;
                rootNode.position = new Vector2(100, 200);
            }

            //注意!!! 深拷贝之后，如果rootNode在nodes中，则会有两个rootNode(tree.Root和nodes中的)
            //这里rootNode的视图和连线要额外处理
            CreateNodeView(tree.root);
            tree.nodes.ForEach(this.CreateNodeView);

            //3. 生成边
            tree.NodeLinkDatas.ForEach(this.CreateEdge);

            //4. 生成背景板
            tree.blockDatas.ForEach(this.CreateCommentBlockView);

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
                    Paste();
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
            RemoveCaches.ForEach(cache =>
            {
                switch (cache)
                {
                    case DialogueNode node:
                        tree.DeleteNode(node);
                        break;
                    case CommentBlockData blockData:
                        tree.DeleteBlock(blockData);
                        break;
                }
            });

            SaveCommentBlock();
            SaveLinkDatas();
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
                    DialogueSettings.GetSettings().copy = nodeView.Clone();
                    break;
                case CommentBlockGroup group:
                    DialogueSettings.GetSettings().copy = new CommentBlockClone(group);
                    break;
            }
        }

        private void Paste()
        {
            switch (DialogueSettings.GetSettings().copy)
            {
                case DialogueNode copyNode:
                    DialogueNode dialogueNode = MongoHelper.Clone(copyNode);
                    CreateNode(dialogueNode);
                    break;
                case CommentBlockClone blockClone:
                    blockClone.Clone(this);
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

        public void CreateNode(DialogueNode node)
        {
            tree.nodes.Add(node);
            CreateNodeView(node);
        }

        private void CreateNodeView(DialogueNode node)
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
                }
            }
        }

        private void SaveNodes()
        {
            List<DialogueNodeView> nodeViews = graphElements.Where(x => x is DialogueNodeView).Cast<DialogueNodeView>().ToList();
            nodeViews.ForEach(nodeview => nodeview.node.TargetID = 0);

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

            nodeViews.ForEach(view => view.SaveCallback?.Invoke());
        }

        #endregion

        #region CommentBlock

        public void CreateCommentBlock(CommentBlockData blockData)
        {
            tree.blockDatas.Add(blockData);
            CreateCommentBlockView(blockData);
        }

        public void CreateCommentBlock(Vector2 position)
        {
            CommentBlockData blockData = tree.CreateBlock(position);
            CreateCommentBlockView(blockData);
        }

        private void CreateCommentBlockView(CommentBlockData blockData)
        {
            var group = new CommentBlockGroup(blockData, this);
            AddElement(group);
            group.SetPosition(new Rect(blockData.position, DefaultCommentSize));
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

        private void RemoveNodeFromGroup(DialogueNodeView nodeView)
        {
            tree.blockDatas.ForEach(group => group.children.Remove(nodeView.viewDataKey));
            this.PopulateView(tree, window);
        }

        #endregion

        #region Edge

        private void SaveLinkDatas()
        {
            tree.NodeLinkDatas.Clear();
            var nodeviews = graphElements.Where(x => x is DialogueNodeView).Cast<DialogueNodeView>().ToList();
            nodeviews.ForEach(nodeView =>
            {
                for (int i = 0; i < nodeView.outports.Count; i++)
                {
                    Port output = nodeView.outports[i];
                    foreach (var edge in output.connections)
                    {
                        DialogueNodeView childView = edge.input.node as DialogueNodeView;
                        tree.NodeLinkDatas.Add(new NodeLinkData()
                        {
                            inputNodeGuid = childView.viewDataKey, outputNodeGuid = nodeView.viewDataKey, portID = i
                        });
                    }
                }
            });
        }

        public void CreateEdge(NodeLinkData linkData)
        {
            DialogueNodeView inputView = GetNodeByGuid(linkData.inputNodeGuid) as DialogueNodeView;
            DialogueNodeView outputView = GetNodeByGuid(linkData.outputNodeGuid) as DialogueNodeView;
            if (inputView == null || outputView == null) return;
            try
            {
                Port outputPort = outputView.outports[linkData.portID];
                Edge edge = outputPort.ConnectTo(inputView.input);
                AddElement(edge);
            }
            catch (Exception e)
            {
                Debug.LogError("index out of range!!!" + e);
            }
        }

        #endregion
    }
}