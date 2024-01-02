using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof(BubbleBaseNode))]
    public sealed class BubbleBaseNodeView : DialogueNodeView
    {
        private readonly Port bubbles;
        
        public BubbleBaseNodeView(DialogueNode node,DialogueTreeView treeView): base(node,treeView)
        {
            this.title = "气泡基类节点";
            this.GenerateInputPort("",true);
            this.bubbles = this.GenerateOutputPort("Bubbles", true);
            this.SaveCallback += this.Save;
        }

        public override void GenerateEdge()
        {
            if(!(this.node is BubbleBaseNode bubbleBaseNode)) return;
            treeView.CreateEdges(this.bubbles,bubbleBaseNode.bubbles);
        }

        private void Save()
        {
            if(!(this.node is BubbleBaseNode bubbleBaseNode)) return;
            bubbleBaseNode.bubbles = this.GetLinkNodes(this.bubbles);
        }
    }
}