using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    public struct test444
    {
        public int a;
    }
    
    [CreateAssetMenu(menuName = "ScriptableObject/DialogueTree/Tree", fileName = "DialogueTree")]
    public class DialogueTree: SerializedScriptableObject
    {
        public string treeName;
        public int treeId;

        [FoldoutGroup("DialogueDatas")]
        [LabelText("根节点")]
        [HideReferenceObjectPicker]
        public DialogueNode root;

        [Space(10)]
        [BsonIgnore]
        [FoldoutGroup("DialogueDatas")]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<DialogueNode> nodes = new();

        [Space(10)]
        [BsonIgnore]
        [FoldoutGroup("DialogueDatas")]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<CommentBlockData> blockDatas = new();
        
        [HideReferenceObjectPicker]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        [DictionaryDrawerSettings(KeyLabel = "TargetID", ValueLabel = "DialogueNode",IsReadOnly = true)]
        public Dictionary<int, DialogueNode> targets = new();
        
        
        public DialogueNode CreateDialogueNode(Type type)
        {
            DialogueNode node = Activator.CreateInstance(type) as DialogueNode;
            node.TargetID = 0;
            node.Guid = GUID.Generate().ToString();
            this.nodes.Add(node);
            EditorUtility.SetDirty(this);
            OnNodeListChanged();
            return node;
        }

        public void OnNodeListChanged()
        {
            this.targets.Clear();
            this.nodes.ForEach(node => { this.targets.TryAdd(node.TargetID, node); });
        }

        private void CreateCommentBlock()
        {
            CommentBlockData blockData = new();
            this.blockDatas.Add(blockData);
            EditorUtility.SetDirty(this);
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
    }
}