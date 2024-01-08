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

            this.title = "根节点";
            outputPort = GenerateOutputPort("start");
        }
    }
}