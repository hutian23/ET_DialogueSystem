using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    public static class VN_ChoiceType
    {
        public const int None = 0;
        public const int Vertification_Normal = 1; // Disco里的白色鉴定
        public const int Vertification_Special = 2; // 红色检定，跳过当前对话就不能
    }

    [NodeType("Visual Novel/选项节点(Visual Novel)")]
    public class VN_ChoiceNode: DialogueNode
    {
        [HideInInspector]
        public uint next;

        [FoldoutGroup("$nodeName/data"), LabelText("选项类型")]
        public int choiceType;
        
        [FoldoutGroup("$nodeName/data"), LabelText("节点执行结果:")]
        public List<NodeCheckConfig> handle_Config = new();
    }
}