namespace ET.Client
{
    public class BBNodeView : DialogueNodeView<BBNode>
    {
        public BBNodeView(BBNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("",true);
        }
    }
}