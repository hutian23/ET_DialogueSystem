﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    [CreateAssetMenu(menuName = "ScriptableObject/DialogueTree",fileName = "DialogueTree")]
    public class DialogueTree : ScriptableObject
    {
        public DialogueNode root;

        [SerializeReference]
        public List<DialogueNode> nodes = new();
        public List<CommentBlockData> blockDatas = new();
        public List<CheckerConfig> checkers = new();
        
        public DialogueNode CreateNode(Type type)
        {
            DialogueNode node = CreateInstance(type) as DialogueNode;
            node.Guid = GUID.Generate().ToString();
            node.name = node.Guid;
            
            if (node == null)
            {
                Debug.LogError($"{type} 不能转换成dialogueNode");
                return null;
            }
            this.nodes.Add(node);
            
            AssetDatabase.AddObjectToAsset(node,this);
            AssetDatabase.SaveAssets();
            
            return node;
        }

        public bool DeleteNode(DialogueNode node)
        {
            if (node == this.root || node.Guid == this.root.Guid)
            {
                return false;
            }

            this.nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
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
    }
}