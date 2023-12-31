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
        // private readonly Vector2 DefaultCommentSize = new(300, 200);

        public new class UxmlFactory: UxmlFactory<DialogueTreeView, UxmlTraits>
        {
        }

        private DialogueTree tree;

        private DialogueEditor window;
        private SearchMenuWindowProvider searchWindow;

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

        public void PopulateView(DialogueTree _tree, DialogueEditor dialogueEditor)
        {
            this.tree = _tree;
            this.window = dialogueEditor;
            DeleteElements(this.graphElements);
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

        public void CreateNode(Type type, Vector2 position)
        {
            DialogueNode node = this.tree.CreateNode(type);
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
                    DialogueNodeView nodeView = Activator.CreateInstance(nodeViewType, args: new object[] { node }) as DialogueNodeView;
                    nodeView.SetPosition(new Rect(node.position, this.DefaultNodeSize));
                    nodeView.GeneratePort();
                    AddElement(nodeView);
                }
            }
        }
    }
}