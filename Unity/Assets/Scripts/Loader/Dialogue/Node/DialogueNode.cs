using System.Collections.Generic;
using ET.Client;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public enum Status
    {
        None,
        Success,
        Pending,
        Failed
    }

    [HideReferenceObjectPicker]
    public abstract class DialogueNode
    {
        [HideInInspector]
        public string Guid;

        [HideInInspector]
        public Vector2 position;

        [BsonIgnore]
        [HideInInspector,ReadOnly,FoldoutGroup("$nodeName")]
        public Status Status;

        [HideInInspector]
        public string text;

        [LabelText("对话树ID"), FoldoutGroup("$nodeName"),ReadOnly]
        public uint TreeID;
        
        [FoldoutGroup("$nodeName"),ReadOnly]
        public uint TargetID;

        [FoldoutGroup("$nodeName")]
        public bool NeedCheck;
        
        [FoldoutGroup("$nodeName"),ShowIf("NeedCheck")]
        public List<NodeCheckConfig> checkList = new();

        public string nodeName => $"[{TargetID}]{GetType().Name}";

#if UNITY_EDITOR
        public virtual DialogueNode Clone()
        {
            DialogueNode cloneNode = MongoHelper.Clone(this);
            cloneNode.TargetID = 0;
            cloneNode.TreeID = 0;
            cloneNode.Guid = GUID.Generate().ToString();
            return cloneNode;
        }
#endif
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