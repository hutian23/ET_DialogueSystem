using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (VN_RandomActionNode))]
    public class VN_RandomActionNodeView: DialogueNodeView
    {
        private readonly Port port;

        public VN_RandomActionNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            this.GenerateInputPort("", true);
            port = this.GenerateOutputPort("");
            this.SaveCallback += this.Save;
        }

        private void Save()
        {
            if (node is not VN_RandomActionNode randomActionNode) return;
            randomActionNode.children = this.GetLinkNodes(port);
        }
    }
}