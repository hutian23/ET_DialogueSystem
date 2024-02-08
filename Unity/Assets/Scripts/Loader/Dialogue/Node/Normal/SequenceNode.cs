using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [NodeType("常用节点/顺序节点")]
    public class SequenceNode: DialogueNode
    {
        [HideInInspector]
        public List<uint> children = new();
    }
}