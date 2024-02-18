using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Blazblue/技能节点")]
    public class BBNode: DialogueNode
    {
        [FoldoutGroup("$nodeName/data"), TextArea(15, 30)]
        public string BBScript = "";
    }
}