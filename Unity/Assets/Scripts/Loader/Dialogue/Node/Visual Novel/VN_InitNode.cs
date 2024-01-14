using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Visual Novel/初始化节点(Visual Novel)")]
    public class VN_InitNode : DialogueNode
    {
        [FoldoutGroup("$nodeName"), Title(title: "初始化脚本", bold: true), HideLabel, TextArea(10, 35)]
        public string Script = "";

        public uint nextNode;
    }
}