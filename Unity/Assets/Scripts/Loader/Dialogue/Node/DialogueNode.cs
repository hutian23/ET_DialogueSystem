﻿using System.Collections.Generic;
using ET.Client;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
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
        [FoldoutGroup("$nodeName")]
        [HideInInspector]
        public string Guid;

        [HideInInspector] 
        public Vector2 position;
        
        [FoldoutGroup("$nodeName")]
        [BsonIgnore]
        [ReadOnly]
        [LabelText("执行状态")]
        public Status Status;

        [HideInInspector]
        public string text;
        
        [ShowInInspector]
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