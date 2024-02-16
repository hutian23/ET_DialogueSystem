using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    public class RootNode: DialogueNode
    {
        [HideInInspector]
        public uint nextNode;

        [FoldoutGroup("$nodeName/data")]
        [LabelText("初始化"), TextArea(10, 35)]
        public string InitScript = "";
    }
}