using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    public class RootNode : DialogueNode
    {
        [FoldoutGroup("$nodeName")]
        public uint nextNode;

        [FoldoutGroup("$nodeName"), Title(title: "脚本", bold: true), HideLabel, TextArea(10, 35)]
        public string Script = "";
    }
}