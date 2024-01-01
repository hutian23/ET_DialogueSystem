using UnityEngine;

namespace ET.Client
{
    public class RootNode : DialogueNode
    {
        [HideInInspector]
        public DialogueNode nextNode;
    }
}