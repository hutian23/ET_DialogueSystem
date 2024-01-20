using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET.Client
{
    [NodeType("Persona/Action节点(Persona)")]
    public class Persona_ActionNode: DialogueNode
    {
        [FoldoutGroup("$nodeName")]
        [ListDrawerSettings(IsReadOnly = true), ReadOnly]
        public List<uint> children = new();
    }
}