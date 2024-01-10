using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET.Client
{
    [NodeType("Persona/谈话入口节点")]
    public class Persona_InitChoiceNode: DialogueNode
    {
        [LabelText("成为伙伴"), FoldoutGroup("$nodeName")]
        [ListDrawerSettings(IsReadOnly = true),ReadOnly]
        public List<uint> Character = new();

        [LabelText("要钱"), FoldoutGroup("$nodeName")]
        [ListDrawerSettings(IsReadOnly = true),ReadOnly]
        public List<uint> Money = new();

        [LabelText("要道具"), FoldoutGroup("$nodeName")]
        [ListDrawerSettings(IsReadOnly = true),ReadOnly]
        public List<uint> Item = new();
    }
}