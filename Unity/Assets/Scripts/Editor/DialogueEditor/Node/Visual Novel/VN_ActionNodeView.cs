using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public sealed class VN_ActionNodeView: DialogueNodeView<VN_ActionNode>
    {
        public VN_ActionNodeView(VN_ActionNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            Port port = GenerateOutputPort("", true);
            SaveCallback += () => { dialogueNode.children = this.GetLinkNodes(port); };
        }
    }
}