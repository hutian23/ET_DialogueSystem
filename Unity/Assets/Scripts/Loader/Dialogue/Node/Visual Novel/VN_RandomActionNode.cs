using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Visual Novel/随机节点(Visual Novel)")]
    public class VN_RandomActionNode : DialogueNode
    {
        [FoldoutGroup("$nodeName/data"),LabelText("最小值: ")]
        public int MinValue = 0;
        [FoldoutGroup("$nodeName/data"),LabelText("最大值: ")]
        public int MaxValue = 0;
        
        [HideInInspector]
        public List<uint> children = new();
    }
}