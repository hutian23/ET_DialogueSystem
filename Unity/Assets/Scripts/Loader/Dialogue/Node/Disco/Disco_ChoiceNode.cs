using UnityEngine;

namespace ET.Client
{
    [NodeType("Disco/分支/检定节点")]
    public class Disco_ChoiceNode: DialogueNode
    {
        //检定成功
        [HideInInspector]
        public int Success;

        //检定失败
        [HideInInspector]
        public int Failed;
    }
}