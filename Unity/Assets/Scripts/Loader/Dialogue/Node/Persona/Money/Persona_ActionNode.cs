using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Persona/怪物")]
    public class Persona_ActionNode: DialogueNode
    {
        [FoldoutGroup("$nodeName")]
        [Title(title: "脚本", bold: true), HideLabel, TextArea(10, 15)]
        public string Script = "";
    }
}