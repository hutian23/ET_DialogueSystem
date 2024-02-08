using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET.Client
{
    [NodeType("常用节点/随机节点")]
    public class RandomNode: DialogueNode
    {
        [FoldoutGroup("$nodeName"), ListDrawerSettings(IsReadOnly = true), ReadOnly]
        public List<uint> random = new();
    }
}