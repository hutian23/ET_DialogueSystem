using System.Collections.Generic;
using ET.Client;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [HideReferenceObjectPicker]
    public abstract class DialogueNode
    {
        [FoldoutGroup("$nodeName")]
        [BsonIgnore]
        public string Guid;

        [HideInInspector]
        [BsonIgnore]
        public Vector2 position;

        [HideInInspector]
        public string text;
        
        [ShowInInspector]
        [BsonIgnore]
        [FoldoutGroup("$nodeName")]
        public int TargetID;
        
        [FoldoutGroup("$nodeName")]
        public bool NeedCheck;

        [ShowInInspector]
        [ShowIf("NeedCheck")]
        [FoldoutGroup("$nodeName")]
        public List<NodeCheckConfig> checkList = new();
        
        public string nodeName => $"[{TargetID}]{GetType().Name}";
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