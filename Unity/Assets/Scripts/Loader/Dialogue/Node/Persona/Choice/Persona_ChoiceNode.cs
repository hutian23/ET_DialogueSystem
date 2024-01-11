using System.Collections.Generic;

namespace ET.Client
{
    [NodeType("Persona/选择节点")]
    public class Persona_ChoiceNode : DialogueNode
    {
        public List<uint> choices = new();
    }
}