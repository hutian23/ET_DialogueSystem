using Sirenix.OdinInspector;

namespace ET.Client
{
    [NodeType("Disco/分支/跳转节点")]
    public class GotoNode : DialogueNode
    {
        [FoldoutGroup("$nodeName")]
        [LabelText("跳转到节点: ")]
        public int Goto_targetID;
    }
}