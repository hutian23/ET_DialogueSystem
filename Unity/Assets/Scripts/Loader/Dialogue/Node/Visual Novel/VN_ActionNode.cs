using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Visual Novel/Action节点(Visual Novel)")]
    public class VN_ActionNode: DialogueNode
    {
        [FoldoutGroup("$nodeName"), Title(title: "脚本", bold: true), HideLabel, TextArea(10, 35)]
        public string Script = "";

        [FoldoutGroup("$nodeName"), ListDrawerSettings(IsReadOnly = true), ReadOnly]
        public List<uint> children = new();
    }
}