using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Visual Novel/延时显示选项板(Visual Novel)")]
    public class VN_Delay_ChoicePanel : DialogueNode
    {
        [HideInInspector]
        public List<uint> normal=new ();

        [HideInInspector]
        public List<uint> special = new();
        
        [FoldoutGroup("$nodeName/data")]
        public int delayTime;
    }
}