using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [NodeType("Disco/分支/愤怒节点（延时显示其他选项）")]
    public class Angry_ChoiceNode: DialogueNode
    {
        [HideInInspector]
        public List<DialogueNode> Angrys = new();
        [HideInInspector]
        public List<DialogueNode> Normal = new();
    }
}