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

        public DialogueTreeView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uss");
            this.styleSheets.Add(styleSheet);
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
                            this.tree.DeleteNode(nodeView.node);
                            break;
                        case CommentBlockGroup group:
                            this.tree.DeleteBlock(group.blockData);
                            break;
                    }
                });
            }
            
            this.SetDirty();
            return graphViewChange;
        }

        public void PopulateView(DialogueTree _tree, DialogueEditor dialogueEditor)
        {
            this.tree = _tree;
            this.window = dialogueEditor;

            this.graphViewChanged -= this.OnGraphViewChanged;
            DeleteElements(this.graphElements);
            this.graphViewChanged += this.OnGraphViewChanged;

            //1. 搜索框
            this.AddSearchWindow();
            //2. 生成视图节点
            //如果没有根节点，则创建
            if (this.tree.root == null)
            {
                DialogueNode rootNode = this.tree.CreateNode(typeof (RootNode));
                this.tree.root = rootNode;
                rootNode.position = new Vector2(100, 200);
            }

            foreach (var node in this.tree.nodes)
            {
                CreateNodeView(node);
            }

            //3. 生成边
            foreach (var node in this.tree.nodes)
            {
                DialogueNodeView nodeView = GetViewFromNode(node);
                nodeView.GenerateEdge();
            }

            //4. 生成背景板
            foreach (var block in this.tree.blockDatas)
            {
                CreateCommentBlockView(block);
            }
        }

        private void AddSearchWindow()
        {
            this.searchWindow = ScriptableObject.CreateInstance<SearchMenuWindowProvider>();
            this.searchWindow.Init(this.window, this);
            //添加回调，按下空格调用
            this.nodeCreationRequest = context =>
            {
                //打开一个searchWindow
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), this.searchWindow);
            };
        }

        public override List<Port> GetCompatiblePorts(Port startPorts, NodeAdapter nodeAdapter)
        {
            var list = this.ports.ToList().Where(endPort =>
                    endPort.direction != startPorts.direction &&
                    endPort.direction != startPorts.direction &&
                    endPort.node != startPorts.node).ToList();
            return list;
        }

        //标记一下视图中有修改
        public void SetDirty()
        {
            this.window.HasUnSave = true;
        }

        #region Node

        /// <summary>
        /// nodeview的viewDataKey = node.Guid
        /// </summary>
        private DialogueNodeView GetViewFromNode(DialogueNode node)
        {
            if (node == null) return null;
            return this.GetNodeByGuid(node.Guid) as DialogueNodeView;
        }

        public void CreateNode(Type type, Vector2 position)
        {
            DialogueNode node = this.tree.CreateNode(type);
            node.position = position;
            Undo.RecordObject(this.tree,"treeSettings");
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
                    nodeView.SetPosition(new Rect(node.position, this.DefaultNodeSize));
                    nodeView.OnNodeSelected += this.OnNodeSelected;
                    AddElement(nodeView);
                }
            }
        }

        public void SaveNodes()
        {
            List<DialogueNodeView> nodeViews = this.graphElements.Where(x => x is DialogueNodeView).Cast<DialogueNodeView>().ToList();
            nodeViews.ForEach(view => view.SaveCallback?.Invoke());
        }

        #endregion

        #region CommentBlock

        public void CreateCommentBlock(Vector2 position)
        {
            CommentBlockData blockData = this.tree.CreateBlock(position);
            CreateCommentBlockView(blockData);
        }

        private void CreateCommentBlockView(CommentBlockData blockData)
        {
            var group = new CommentBlockGroup(blockData, this);
            AddElement(group);
            group.SetPosition(new Rect(blockData.position, this.DefaultCommentSize));
            foreach (var guid in group.blockData.children)
            {
                DialogueNodeView nodeView = GetNodeByGuid(guid) as DialogueNodeView;
                if (nodeView == null) continue;
                group.AddElement(nodeView);
            }
        }

        public void SaveCommentBlock()
        {
            List<CommentBlockGroup> groups = this.graphElements.Where(x => x is CommentBlockGroup).Cast<CommentBlockGroup>().ToList();
            groups.ForEach(block => block.Save());
            EditorUtility.SetDirty(this.tree);
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
            DialogueNodeView nodeView = this.GetViewFromNode(childNode);
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
                DialogueNodeView nodeView = this.GetViewFromNode(node);
                if (nodeView == null) continue;
                Edge edge = output.ConnectTo(nodeView.input);
                AddElement(edge);
            }
        }

        #endregion
    }
}