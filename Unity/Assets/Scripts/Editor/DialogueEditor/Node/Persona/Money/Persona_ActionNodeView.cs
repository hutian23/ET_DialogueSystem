namespace ET.Client
{
    [NodeEditorOf(typeof(Persona_ActionNode))]
    public class Persona_ActionNodeView : DialogueNodeView
    {
        public Persona_ActionNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            this.GenerateInputPort("", true);
            this.GenerateOutputPort("",true);
            this.GenerateDescription();
        }
    }
}