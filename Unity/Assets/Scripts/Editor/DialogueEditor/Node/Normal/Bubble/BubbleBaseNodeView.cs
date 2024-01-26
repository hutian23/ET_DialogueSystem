using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public sealed class BubbleBaseNodeView : DialogueNodeView<BubbleBaseNode>
    {
        
        public BubbleBaseNodeView(BubbleBaseNode node,DialogueTreeView treeView): base(node,treeView)
        {
            GenerateInputPort("",true);
            Port bubbles = GenerateOutputPort("Bubbles", true);
            SaveCallback += () => { node.bubbles = GetLinkNodes(bubbles);};
        }
    }
}