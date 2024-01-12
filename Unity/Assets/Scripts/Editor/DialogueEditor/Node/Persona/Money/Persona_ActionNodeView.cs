using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (Persona_ActionNode))]
    public class Persona_ActionNodeView: DialogueNodeView
    {
        private readonly Port port;

        public Persona_ActionNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            this.GenerateInputPort("", true);
            port = this.GenerateOutputPort("", true);
            this.SaveCallback += Save;
        }

        private void Save()
        {
            if (node is not Persona_ActionNode actionNode) return;
            actionNode.children = this.GetLinkNodes(port);
        }
    }
}