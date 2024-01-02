namespace ET.Client
{
    [NodeEditorOf(typeof(BubbleActionNode))]
    public sealed class BubbleActionNodeView : DialogueNodeView
    {
        public BubbleActionNodeView(DialogueNode node,DialogueTreeView treeView): base(node,treeView)
        {
            this.title = "气泡子节点";

            this.GenerateInputPort("");
            this.GenerateDescription();
        }

        public override void GenerateEdge()
        {
        }
    }
}