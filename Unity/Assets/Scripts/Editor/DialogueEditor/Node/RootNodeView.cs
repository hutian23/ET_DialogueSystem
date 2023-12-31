using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof(RootNode))]
    public sealed class RootNodeView : DialogueNodeView
    {
        public Port outputPort;
        
        public RootNodeView(DialogueNode node): base(node)
        {
            this.capabilities &= ~ Capabilities.Movable;
            this.capabilities &= ~ Capabilities.Deletable;

            this.title = "根节点";
            
            this.outputPort = GenerateOutputPort("开始");
            this.outputContainer.Add(this.outputPort);
        }
    }
}