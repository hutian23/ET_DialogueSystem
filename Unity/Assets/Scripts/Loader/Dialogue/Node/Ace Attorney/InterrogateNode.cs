using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Ace Attorney/讯问节点")]
    public class InterrogateNode: DialogueNode
    {
        [HideInInspector]
        public uint hold_it;

        [HideInInspector]
        public uint take_that_Success;

        [HideInInspector]
        public uint take_that_Failed;

        [HideInInspector]
        public List<uint> nexts = new();

        [FoldoutGroup("$nodeName"), LabelText("回退到: "), Space(10)]
        public uint preNode;
    }
}