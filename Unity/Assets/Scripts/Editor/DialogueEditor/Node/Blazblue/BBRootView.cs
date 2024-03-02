namespace ET.Client
{
    public class BBRootView: DialogueNodeView<BBRoot>
    {
        public BBRootView(BBRoot dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("");
            GenerateOutputPort("", true);
        }
    }
}