using UnityEngine;

namespace ET.Client
{
    [NodeType("Blazblue/技能节点")]
    public class BBNode: DialogueNode
    {
        [HideInInspector]
        public string BBScript = "";
    }
}