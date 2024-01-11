using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof(Persona_ChoiceNode))]
    public class Persona_ChoiceNodeView : DialogueNodeView
    {
        private readonly Port choices;
        public Persona_ChoiceNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            this.GenerateInputPort("", true);
            choices = this.GenerateOutputPort("", true);

            this.SaveCallback += this.Save;
        }

        private void Save()
        {
            if(node is not Persona_ChoiceNode choiceNode)return;
            choiceNode.choices = this.GetLinkNodes(choices);
        }
    }
}