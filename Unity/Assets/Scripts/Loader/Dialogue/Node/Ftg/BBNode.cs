using Sirenix.OdinInspector;

namespace ET.Client
{
    [NodeType("Blazblue/技能节点")]
    public class BBNode: DialogueNode
    {
        [FoldoutGroup("$nodeName/data")]
        public string BBScript = "";
    }
}