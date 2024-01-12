using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Persona/Action节点(Persona)")]
    public class Persona_ActionNode: DialogueNode
    {
        [FoldoutGroup("$nodeName")]
        [Title(title: "脚本", bold: true), HideLabel, TextArea(10, 15)]
        public string Script = "";

        [FoldoutGroup("$nodeName")]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<uint> children = new();
    }
}