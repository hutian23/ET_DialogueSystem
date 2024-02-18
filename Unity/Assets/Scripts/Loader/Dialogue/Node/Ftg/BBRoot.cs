using System.Collections.Generic;

namespace ET.Client
{
    [NodeType("Blazblue/行为机根节点")]
    public class BBRoot: DialogueNode
    {
        public List<uint> behaviors = new();
    }
}