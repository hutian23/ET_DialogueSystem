using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (VN_InitNode))]
    public class VN_InitNodeView: DialogueNodeView
    {
        private readonly Port port;

        public VN_InitNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            this.GenerateInputPort("");
            port = this.GenerateOutputPort("");
            this.SaveCallback += this.Save;
        }

        private void Save()
        {
            if (node is not VN_InitNode initNode) return;
            initNode.nextNode = this.GetFirstLinkNode(port);
        }
    }
}