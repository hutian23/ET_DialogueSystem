﻿using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    [CreateAssetMenu(menuName = "ScriptableObject/DialogueTree/Tree", fileName = "DialogueTree")]
    public class DialogueTree: SerializedScriptableObject
    {
        public string treeName;
        public uint treeID;

        [FoldoutGroup("DialogueDatas")]
        [LabelText("根节点")]
        [HideReferenceObjectPicker]
        public DialogueNode root;

        [Space(10)]
        [FoldoutGroup("DialogueDatas")]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<DialogueNode> nodes = new();

        [Space(10)]
        [FoldoutGroup("DialogueDatas")]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<CommentBlockData> blockDatas = new();

        [Space(10)]
        [FoldoutGroup("DialogueDatas")]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<NodeLinkData> NodeLinkDatas = new();

        [HideReferenceObjectPicker]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        [DictionaryDrawerSettings(KeyLabel = "TargetID", ValueLabel = "DialogueNode", IsReadOnly = true)]
        public Dictionary<uint, DialogueNode> targets = new();

        public RootNode CreateRoot()
        {
            RootNode rootNode = Activator.CreateInstance<RootNode>();
            rootNode.TargetID = 0;
            rootNode.TreeID = this.treeID;
            rootNode.Guid = GUID.Generate().ToString();
            rootNode.nextNode = 1;
            EditorUtility.SetDirty(this);
            return rootNode;
        }

        public DialogueNode CreateDialogueNode(Type type)
        {
            DialogueNode node = Activator.CreateInstance(type) as DialogueNode;
            node.TargetID = 0;
            node.TreeID = this.treeID;
            node.Guid = GUID.Generate().ToString();
            this.nodes.Add(node);
            EditorUtility.SetDirty(this);
            return node;
        }

        #region old

        public bool DeleteNode(DialogueNode node)
        {
            if (node == this.root || node.Guid == this.root.Guid)
            {
                return false;
            }

            this.nodes.Remove(node);
            return true;
        }

        public CommentBlockData CreateBlock(Vector2 position)
        {
            CommentBlockData blockData = new() { position = position, title = "Comment Block" };
            this.blockDatas.Add(blockData);
            EditorUtility.SetDirty(this);
            return blockData;
        }

        public void DeleteBlock(CommentBlockData blockData)
        {
            this.blockDatas.Remove(blockData);
            EditorUtility.SetDirty(this);
        }

        #endregion

        public DialogueTree DeepClone()
        {
            DialogueTree cloneTree = MongoHelper.Clone(this);
            cloneTree.targets.Clear();
            cloneTree.targets.Add(0, cloneTree.root);
            cloneTree.nodes.ForEach(node => { cloneTree.targets.TryAdd(node.TargetID, node); });
            return cloneTree;
        }

        public DialogueTarget CloneTargets()
        {
            //TODO 想想怎么忽略guid，position这些字段
            DialogueTarget targetsDict = new DialogueTarget() { targets = this.targets };
            return MongoHelper.Clone(targetsDict);
        }
    }

    public class DialogueTarget
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<uint, DialogueNode> targets = new();
    }
}