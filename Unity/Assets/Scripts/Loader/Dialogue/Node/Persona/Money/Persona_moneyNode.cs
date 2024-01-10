using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET.Client
{
    [NodeType("Persona/检定节点(要钱)")]
    public class Persona_moneyNode: DialogueNode
    {
        [FoldoutGroup("$nodeName")]
        public uint MinValue;

        [FoldoutGroup("$nodeName")]
        public uint MaxValue;

        [FoldoutGroup("$nodeName")]
        [ReadOnly]
        public List<uint> Success = new();

        [FoldoutGroup("$nodeName")]
        [ReadOnly]
        public List<uint> Failed = new();

        public int test = 123;
    }
}