using System.Collections.Generic;

namespace ET.Client
{
    [NodeType("常用节点/随机节点")]
    public class RandomNode : DialogueNode
    {
        public List<uint> random = new();
    }
}