using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public sealed class Persona_ChoiceNodeView: DialogueNodeView<Persona_ChoiceNode>
    {
        public Persona_ChoiceNodeView(Persona_ChoiceNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            Port choices = GenerateOutputPort("", true);

            SaveCallback += () => { dialogueNode.choices = GetLinkNodes(choices); };
        }
    }
}