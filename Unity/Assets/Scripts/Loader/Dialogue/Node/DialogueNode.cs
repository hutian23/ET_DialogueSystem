using System;
using System.Collections.Generic;
using ET.Client;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
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
    
    public enum Language
    {
        CN,
        EN,
        JPN,
        ALL
    }

    [HideReferenceObjectPicker]
    public abstract class DialogueNode
    {
        [HideInInspector, ReadOnly]
        public uint TreeID;

        [HideInInspector, ReadOnly]
        public uint TargetID;

        [FoldoutGroup("$nodeName")]
        public bool NeedCheck = false;

        [FoldoutGroup("$nodeName"), ShowIf("NeedCheck")]
        public List<NodeCheckConfig> checkList = new();

        [FoldoutGroup("$nodeName"), Title(title: "脚本", bold: true), HideLabel, TextArea(10, 35)]
        public string Script = "";

#if UNITY_EDITOR
        [HideInInspector]
        public string Guid;

        [HideInInspector]
        public Vector2 position;

        [BsonIgnore]
        [HideInInspector, ReadOnly, FoldoutGroup("$nodeName")]
        public Status Status;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        [LabelText("文本"), FoldoutGroup("$nodeName")]
        public Dictionary<Language, string> contents = new();

        public string nodeName => $"[{TargetID}]{GetType().Name}";

        public virtual DialogueNode Clone()
        {
            DialogueNode cloneNode = MongoHelper.Clone(this);
            cloneNode.TargetID = 0;
            cloneNode.TreeID = 0;
            cloneNode.Guid = GUID.Generate().ToString();
            return cloneNode;
        }
#endif

        public long GetID()
        {
            ulong result = 0;
            result |= TargetID;
            result |= (ulong)TreeID << 32;
            return (long)result;
        }
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