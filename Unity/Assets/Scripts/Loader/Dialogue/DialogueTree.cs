using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    [CreateAssetMenu(menuName = "ScriptableObject/DialogueTree", fileName = "DialogueTree")]
    public class DialogueTree: SerializedScriptableObject
    {
        public string treeName;
        public int treeId;

        [ReadOnly]
        public DialogueNode root;

        public List<DialogueNode> nodes = new();
        public List<CommentBlockData> blockDatas = new();

        [ShowInInspector]
        [ReadOnly]
        [DictionaryDrawerSettings(KeyLabel = "TargetID", ValueLabel = "DialogueNode")]
        public Dictionary<int, DialogueNode> targets = new();

        [Space(10)]
        [InfoBox("最好不要手动修改TargetID,如果修改了记得也要改IdGenerator")]
        public int IdGenerator;

        private int GenerateId()
        {
            return IdGenerator++;
        }
        
        private void CreateDialogueNode()
        {
            DialogueNode node = new RootNode() { TargetID = GenerateId(), Guid = GUID.Generate().ToString() };
            this.nodes.Add(node);
            EditorUtility.SetDirty(this);
            OnNodeListChanged();
        }

        private void OnNodeListChanged()
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

        public DialogueNode CreateNode(Type type)
        {
            DialogueNode node = Activator.CreateInstance(type) as DialogueNode;
            node.Guid = GUID.Generate().ToString();
            // node.name = node.Guid;
            node.TargetID = GenerateId();

            // if (node == null)
            // {
            //     Debug.LogError($"{type} 不能转换成dialogueNode");
            //     return null;
            // }

            this.nodes.Add(node);

            // AssetDatabase.AddObjectToAsset(node, this);
            // AssetDatabase.SaveAssets();

            return node;
        }

        public bool DeleteNode(DialogueNode node)
        {
            if (node == this.root || node.Guid == this.root.Guid)
            {
                return false;
            }

            this.nodes.Remove(node);
            // AssetDatabase.RemoveObjectFromAsset(node);
            // AssetDatabase.SaveAssets();
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