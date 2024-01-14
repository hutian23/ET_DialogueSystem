using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof(VN_ActionNode))]
    public class VN_ActionNodeView: DialogueNodeView
    {
        private readonly Port port;

        public VN_ActionNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            this.GenerateTextAera();
            this.GenerateInputPort("", true);
            port = this.GenerateOutputPort("", true);
            this.SaveCallback += this.Save;
        }

        private void Save()
        {
            if (node is not VN_ActionNode actionNode) return;
            actionNode.children = this.GetLinkNodes(port);
        }
    }
}