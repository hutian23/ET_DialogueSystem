using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Visual Novel/随机节点(Visual Novel)")]
    public class VN_RandomActionNode : DialogueNode
    {
        [FoldoutGroup("$nodeName"),Title("最小值")]
        public int MinValue;
        [FoldoutGroup("$nodeName"),Title("最大值")]
        public int MaxValue;
        
        [FoldoutGroup("$nodeName"), Title(title: "脚本", bold: true), HideLabel, TextArea(10, 35)]
        public string Script = "";
        
        [FoldoutGroup("$nodeName"), ListDrawerSettings(IsReadOnly = true), ReadOnly]
        public List<uint> children = new();
    }
}