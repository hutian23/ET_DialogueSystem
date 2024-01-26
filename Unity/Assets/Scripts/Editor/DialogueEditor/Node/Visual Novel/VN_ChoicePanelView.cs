using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public class VN_ChoicePanelView: DialogueNodeView<VN_ChoicePanel>
    {
        public VN_ChoicePanelView(VN_ChoicePanel dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            Port port = GenerateOutputPort("选项", true);
            this.SaveCallback += () => { dialogueNode.children = GetLinkNodes(port); };
        }
    }
}