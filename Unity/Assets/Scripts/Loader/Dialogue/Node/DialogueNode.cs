using UnityEngine;

namespace ET
{
    public abstract class DialogueNode : ScriptableObject
    {
        [HideInInspector]
        public string Guid;
        [HideInInspector]
        public Vector2 position;
        
        public bool NeedCheck;
        [TextArea]
        public string text;
    }
    
    public class NodeTypeAttribute : BaseAttribute
    {
        public string Level;
        
        public NodeTypeAttribute(string level)
        {
            this.Level = level;
        }
    }
}