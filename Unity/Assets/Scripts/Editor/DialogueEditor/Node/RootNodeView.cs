using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (RootNode))]
    public sealed class RootNodeView: DialogueNodeView
    {
        private readonly Port outputPort;

        public RootNodeView(DialogueNode node,DialogueTreeView treeView): base(node,treeView)
        { 
            capabilities &= ~ Capabilities.Movable;
            capabilities &= ~ Capabilities.Deletable;

            title = "根节点";
            outputPort = GenerateOutputPort("start");

            SaveCallback += this.Save;
        }

        public override void GenerateEdge()
        {
            if (!(node is RootNode rootNode)) return;
            treeView.CreateEdge(outputPort, rootNode.nextNode);
        }

        private void Save()
        {
            if (!(node is RootNode rootNode)) return;
            rootNode.nextNode = GetFirstLinkNode(outputPort);
        }
    }
}