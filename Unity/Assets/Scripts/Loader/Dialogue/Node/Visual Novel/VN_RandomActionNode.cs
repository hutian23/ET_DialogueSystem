using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Visual Novel/随机节点(Visual Novel)")]
    public class VN_RandomActionNode : DialogueNode
    {
        [FoldoutGroup("$nodeName/data"),LabelText("是否使用外部变量?: ")]
        public bool UseSharedVariable;
        public bool sharedMode => UseSharedVariable == false;
        public bool innerMode => UseSharedVariable;
        
        [FoldoutGroup("$nodeName/data"),LabelText("最小值: "),ShowIf("sharedMode")]
        public int MinValue = 0;
        [FoldoutGroup("$nodeName/data"),LabelText("最大值: "),ShowIf("sharedMode")]
        public int MaxValue = 0;

        [FoldoutGroup("$nodeName/data"),LabelText("最小变量: "),ShowIf("innerMode")]
        public string minVariable;
        [FoldoutGroup("$nodeName/data"),LabelText("最大变量: "),ShowIf("innerMode")]
        public string maxVariable;
        
        [HideInInspector]
        public List<uint> children = new();
    }
}