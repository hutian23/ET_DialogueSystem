using System.Collections.Generic;
using ET.Client;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public abstract class DialogueNode
    {
        [HideInInspector]
        public string Guid;

        [HideInInspector]
        public Vector2 position;

        [HideInInspector]
        public string text;

        public int TargetID;
        public bool NeedCheck;

        [ShowInInspector]
        [ShowIf("NeedCheck")]
        public List<NodeCheckConfig> checkList = new();
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