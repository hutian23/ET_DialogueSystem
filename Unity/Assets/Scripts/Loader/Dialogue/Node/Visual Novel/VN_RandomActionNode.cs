using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET.Client
{
    [NodeType("Visual Novel/随机节点(Visual Novel)")]
    public class VN_RandomActionNode : DialogueNode
    {
        [FoldoutGroup("$nodeName"),Title("最小值")]
        public int MinValue;
        [FoldoutGroup("$nodeName"),Title("最大值")]
        public int MaxValue;
        
        [FoldoutGroup("$nodeName"), ListDrawerSettings(IsReadOnly = true), ReadOnly]
        public List<uint> children = new();
    }
}