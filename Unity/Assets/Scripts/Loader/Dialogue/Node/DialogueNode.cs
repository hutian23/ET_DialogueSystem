using ET.Client;
using UnityEngine;

namespace ET
{
    public abstract class DialogueNode: ScriptableObject
    {
        [HideInInspector]
        public string Guid;

        [HideInInspector]
        public Vector2 position;
        
        [HideInInspector]
        public string text;

        public bool NeedCheck;
        public NodeChecker NodeChecker;
    }

    public class NodeTypeAttribute: BaseAttribute
    {
        public string Level;

        public NodeTypeAttribute(string level)
        {
            this.Level = level;
        }
    }
}