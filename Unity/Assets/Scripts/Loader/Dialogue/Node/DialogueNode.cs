using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public abstract class DialogueNode : ScriptableObject
    {
        [HideInInspector]
        public long Guid;
        [HideInInspector]
        public List<DialogueNode> children = new();
        [HideInInspector]
        public Vector2 position;
        
        [TextArea]
        public string text;
    }

    public class NodeTypeAttribute : BaseAttribute
    {
        public string Level;
        public string Description;
        
        public NodeTypeAttribute(string level,string Description="")
        {
            this.Level = level;
            this.Description = Description;
        }
    }
}