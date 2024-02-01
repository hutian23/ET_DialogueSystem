using UnityEditor.Experimental.GraphView;
namespace ET.Client
{
    public sealed class VN_RandomActionNodeView: DialogueNodeView<VN_RandomActionNode>
    {
        public VN_RandomActionNodeView(VN_RandomActionNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            Port port = GenerateOutputPort("");
            SaveCallback += () => { dialogueNode.next = GetFirstLinkNode(port); };
        }
    }
}