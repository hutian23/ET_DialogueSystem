using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [NodeType("常用节点/气泡/气泡基类节点")]
    public class BubbleBaseNode: DialogueNode
    {
        [HideInInspector]
        public List<DialogueNode> bubbles = new();
    }
}