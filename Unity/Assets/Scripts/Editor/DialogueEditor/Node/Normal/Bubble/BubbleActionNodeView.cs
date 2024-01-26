namespace ET.Client
{
    public sealed class BubbleActionNodeView : DialogueNodeView<BubbleActionNode>
    {
        public BubbleActionNodeView(BubbleActionNode node,DialogueTreeView treeView): base(node,treeView)
        {
            GenerateInputPort("");
        }
    }
}