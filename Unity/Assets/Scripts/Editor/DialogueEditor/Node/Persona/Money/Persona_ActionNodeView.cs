using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public sealed class Persona_ActionNodeView: DialogueNodeView<Persona_ActionNode>
    {
        private readonly Port port;

        public Persona_ActionNodeView(Persona_ActionNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            port = GenerateOutputPort("", true);
            SaveCallback += Save;
        }

        private void Save()
        {
            if (node is not Persona_ActionNode actionNode) return;
            actionNode.children = GetLinkNodes(port);
        }
    }
}