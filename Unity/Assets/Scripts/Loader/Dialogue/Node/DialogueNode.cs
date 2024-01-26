using System;
using System.Collections.Generic;
using System.Linq;
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

    public enum Language
    {
        Chinese,
        English,
        Japanese
    }

    [HideReferenceObjectPicker]
    public abstract class DialogueNode: Object
    {
        [HideInInspector, ReadOnly]
        public uint TreeID;

        [HideInInspector, ReadOnly]
        public uint TargetID;

        [FoldoutGroup("$nodeName"),LabelText("前置条件: ")]
        public bool NeedCheck = false;

        [FoldoutGroup("$nodeName"), ShowIf("NeedCheck"),HideLabel]
        public List<NodeCheckConfig> checkList = new();

        [FoldoutGroup("$nodeName"), LabelText("显示脚本: ")]
        [BsonIgnore]
        public bool ShowScript;
        
        [FoldoutGroup("$nodeName"), HideLabel, TextArea(10, 35),ShowIf("ShowScript")]
        public string Script = "";

        [HideInInspector, BsonIgnore]
        public string text;

#if UNITY_EDITOR
        [HideInInspector]
        public string Guid;

        [HideInInspector]
        public Vector2 position;

        [BsonIgnore]
        [HideInInspector, ReadOnly, FoldoutGroup("$nodeName")]
        public Status Status;

        [Searchable]
        [FoldoutGroup("$nodeName"), HideReferenceObjectPicker, LabelText("本地化"), Space(10),
         ListDrawerSettings(ShowFoldout = true, ShowIndexLabels = true, ListElementLabelName = "eleName")]
        public List<LocalizationGroup> LocalizationGroups = new();

        public string nodeName => $"[{TargetID}]{GetType().Name}";

        public string GetContent(Language language)
        {
            //所以我明明new了，为什么odin还会显示null呢?
            if (LocalizationGroups == null) return String.Empty;
            var targetGroup = LocalizationGroups.FirstOrDefault(group => group.Language == language);
            return targetGroup == null? String.Empty : targetGroup.content;
        }

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

        //ID转成treeID和TargetID
        public void FromID(long ID)
        {
            ulong result = (ulong)ID;
            TargetID = (uint)(result & uint.MaxValue);
            result >>= 32;
            TreeID = (uint)(result & uint.MaxValue);
        }
    }

#if UNITY_EDITOR
    [Serializable]
    public class LocalizationGroup
    {
        [LabelText("语言: "), Space(10)]
        public Language Language = Language.Chinese;

        public string eleName => Language.ToString();

        [TextArea(3, 4), Space(10)]
        [HideLabel]
        public string content = "";
    }
#endif

    public class NodeTypeAttribute: BaseAttribute
    {
        public string Level;

        public NodeTypeAttribute(string level)
        {
            this.Level = level;
        }
    }
}