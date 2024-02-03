using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    public enum VN_ChoiceType
    {
        None = 0,
        Vertification_Normal = 1, // Disco里的白色鉴定
        Vertification_Special = 2 // 红色检定，跳过当前对话就不能
    }

    [NodeType("Visual Novel/选项节点(Visual Novel)")]
    public class VN_ChoiceNode: DialogueNode
    {
        [HideInInspector]
        public uint next;

        [FoldoutGroup("$nodeName/data"), LabelText("选项类型")]
        public VN_ChoiceType choiceType;

        public bool IsVertification => this.choiceType is VN_ChoiceType.Vertification_Normal or VN_ChoiceType.Vertification_Special;

        [Space(10), FoldoutGroup("$nodeName/data"), LabelText("节点执行结果:"), ShowIf("$IsVertification")]
        public List<NodeCheckConfig> handle_Configs = new();
    }
}