using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [NodeType("常用节点/随机节点")]
    public class RandomNode: DialogueNode
    {
        [HideInInspector]
        public List<uint> random = new();
    }
    
}