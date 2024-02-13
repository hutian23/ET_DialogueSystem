using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public class VN_Delay_ChoicePanelNodeView : DialogueNodeView<VN_Delay_ChoicePanel>
    {
        public VN_Delay_ChoicePanelNodeView(VN_Delay_ChoicePanel dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            Port port = GenerateOutputPort("正常显示: ", true);
            Port port2 = GenerateOutputPort("延时显示: ", true);
            SaveCallback += () =>
            {
                dialogueNode.normal = GetLinkNodes(port);
                dialogueNode.special = GetLinkNodes(port2);
            };
        }
    }
}