namespace ET.Client
{
    [NodeEditorOf(typeof(BubbleActionNode))]
    public sealed class BubbleActionNodeView : DialogueNodeView
    {
        public BubbleActionNodeView(DialogueNode node): base(node)
        {
            this.title = "气泡子节点";

            this.GenerateInputPort("");
            this.GenerateDescription();
        }

        public override void GenerateEdge(DialogueTreeView treeView)
        {
        }
    }
}