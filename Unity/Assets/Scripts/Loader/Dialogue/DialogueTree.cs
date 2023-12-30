using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [CreateAssetMenu(menuName = "ScriptableObject/DialogueTree",fileName = "DialogueTree")]
    public class DialogueTree : ScriptableObject
    {
        public DialogueNode root;

        public List<DialogueNode> nodes = new();
    }
}