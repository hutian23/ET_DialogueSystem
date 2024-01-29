using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    public class RootNode : DialogueNode
    {
        [HideInInspector]
        public uint nextNode;
    }
}