namespace ET.Client
{
    [NodeEditorOf(typeof(BubbleActionNode))]
    public sealed class BubbleActionNodeView : DialogueNodeView
    {
        public BubbleActionNodeView(DialogueNode node,DialogueTreeView treeView): base(node,treeView)
        {
            this.GenerateInputPort("");
        }
    }
}