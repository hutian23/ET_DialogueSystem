using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public sealed class RootNodeView: DialogueNodeView<RootNode>
    {
        public RootNodeView(RootNode node,DialogueTreeView treeView): base(node,treeView)
        { 
            capabilities &= ~ Capabilities.Movable;
            capabilities &= ~ Capabilities.Deletable;

            title = "根节点";
            GenerateOutputPort("start");
        }
    }
}