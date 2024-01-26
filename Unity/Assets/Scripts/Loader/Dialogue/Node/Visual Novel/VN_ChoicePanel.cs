using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Visual Novel/选项板(Visual Novel)")]
    public class VN_ChoicePanel : DialogueNode
    {
        [HideInInspector]
        public List<uint> children = new();
        
        [LabelText("检查子节点前置条件: "),FoldoutGroup("$nodeName/data")]
        public bool checkChildren = false;
    }
}