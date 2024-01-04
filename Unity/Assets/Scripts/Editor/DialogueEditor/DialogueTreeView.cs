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

        public Action<DialogueNodeView> OnNodeSelected;

        // private List<DialogueNode> removeCaches = new();
        // private List<CommentBlockData> blockDatasCaches = new();

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

            // //当前选中的元素
            // selection.OfType<DialogueNodeView>();
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
                            tree.DeleteNode(nodeView.node);
                            break;
                        case CommentBlockGroup group:
                            tree.DeleteBlock(group.blockData);
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

            // removeCaches.Clear();
            // blockDatasCaches.Clear();

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            //1. 搜索框
            AddSearchWindow();
            //2. 生成视图节点
            //如果没有根节点，则创建
            if (tree.root == null)
            {
                DialogueNode rootNode = tree.CreateNode(typeof (RootNode));
                tree.root = rootNode;
                rootNode.position = new Vector2(100, 200);
            }

            foreach (var node in tree.nodes)
            {
                CreateNodeView(node);
            }

            //3. 生成边
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
                    //不能通过鼠标获取Group
                    // case CommentBlockGroup blockGroup:
                    //     evt.menu.AppendAction("Remove all child",_=>{Debug.Log("remove all child");});
                    //     break;
                }
            }
            evt.menu.AppendAction("Redo",_=> this.OnRedo());
        }

        private void RemoveNodeFromGroup(DialogueNodeView nodeView)
        {
            foreach (var group in tree.blockDatas)
            {
                group.children.Remove(nodeView.viewDataKey);
            }
            this.PopulateView(tree,window);
        }
        
        public override List<Port> GetCompatiblePorts(Port startPorts, NodeAdapter nodeAdapter)
        {
            var list = ports.ToList().Where(endPort =>
                    endPort.direction != startPorts.direction &&
                    endPort.direction != startPorts.direction &&
                    endPort.node != startPorts.node).ToList();
            return list;
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
                    window.SaveDialogueTree();
                    evt.StopPropagation();
                    break;
                //撤销
                case KeyCode.Z:
                    OnRedo();
                    evt.StopPropagation();
                    break;
            }
        }

        private void OnRedo()
        {
            SetDirty(false);
            PopulateView(tree,window);
        }
        
        //标记一下视图中有修改
        public void SetDirty(bool HasUnSave = true)
        {
            window.HasUnSave = HasUnSave;
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
            DialogueNode node = tree.CreateNode(type);
            node.position = position;
            CreateNodeView(node);
        }

        /// <summary>
        /// 创建视图节点
        /// </summary>
        /// <param name="node"></param>
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
                    nodeView.OnNodeSelected += OnNodeSelected;
                    AddElement(nodeView);
                }
            }
        }

        public void SaveNodes()
        {
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
            AddElement(group);
            group.SetPosition(new Rect(blockData.position, DefaultCommentSize));
            foreach (var guid in group.blockData.children)
            {
                DialogueNodeView nodeView = GetNodeByGuid(guid) as DialogueNodeView;
                if (nodeView == null) continue;
                group.AddElement(nodeView);
            }
        }

        public void SaveCommentBlock()
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
        /// <param name="output"></param>
        /// <param name="childNode"></param>
        /// <returns></returns>
        public Edge CreateEdge(Port output, DialogueNode childNode)
        {
            if (output == null || output.direction == Direction.Input || childNode == null) return null;
            DialogueNodeView nodeView = GetViewFromNode(childNode);
            if (nodeView == null) return null;

            Edge edge = output.ConnectTo(nodeView.input);
            AddElement(edge);
            return edge;
        }

        public void CreateEdges(Port output, List<DialogueNode> nodeList)
        {
            if (output == null || output.direction == Direction.Input || nodeList == null) return;
            foreach (var node in nodeList)
            {
                DialogueNodeView nodeView = GetViewFromNode(node);
                if (nodeView == null) continue;
                Edge edge = output.ConnectTo(nodeView.input);
                AddElement(edge);
            }
        }

        #endregion
    }
}