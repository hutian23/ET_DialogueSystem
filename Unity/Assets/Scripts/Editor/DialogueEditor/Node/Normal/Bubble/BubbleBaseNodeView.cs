using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof(BubbleBaseNode))]
    public sealed class BubbleBaseNodeView : DialogueNodeView
    {
        private readonly Port bubbles;
        
        public BubbleBaseNodeView(DialogueNode node,DialogueTreeView treeView): base(node,treeView)
        {
            GenerateInputPort("",true);
            bubbles = GenerateOutputPort("Bubbles", true);
            SaveCallback += Save;
        }

        public override void GenerateEdge()
        {
            if(!(node is BubbleBaseNode bubbleBaseNode)) return;
            treeView.CreateEdges(bubbles,bubbleBaseNode.bubbles);
        }

        private void Save()
        {
            if(!(node is BubbleBaseNode bubbleBaseNode)) return;
            bubbleBaseNode.bubbles = GetLinkNodes(bubbles);
        }

        public override DialogueNode Clone()
        {
            BubbleBaseNode baseNode = node as BubbleBaseNode;
            baseNode.bubbles.Clear();
            return base.Clone();
        }
    }
}