using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Blazblue/行为节点")]
    public class BBNode: DialogueNode
    {
        [LabelText("行为: ")]
        [FoldoutGroup("$nodeName/data")]
        public string behaviorName;

        [HideInInspector]
        public string BBScript = "";
    }
}