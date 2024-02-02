using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public class VN_ChoiceNodeView: DialogueNodeView<VN_ChoiceNode>
    {
        public VN_ChoiceNodeView(VN_ChoiceNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("");
            Port outport = GenerateOutputPort("");
            SaveCallback += () => { dialogueNode.next = GetFirstLinkNode(outport); };
        }
    }
}