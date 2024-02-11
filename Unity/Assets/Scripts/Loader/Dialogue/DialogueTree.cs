using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    [CreateAssetMenu(menuName = "ScriptableObject/DialogueTree/Tree", fileName = "DialogueTree")]
    public class DialogueTree: SerializedScriptableObject
    {
        public string treeName;

        [Range(1, 10000)]
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

        public List<SharedVariable> Variables = new();

        public RootNode CreateRoot()
        {
            RootNode rootNode = Activator.CreateInstance<RootNode>();
            rootNode.TargetID = 0;
            rootNode.TreeID = treeID;
            rootNode.Guid = GUID.Generate().ToString();
            rootNode.nextNode = 1;
            EditorUtility.SetDirty(this);
            return rootNode;
        }

        public DialogueNode CreateDialogueNode(Type type)
        {
            DialogueNode node = Activator.CreateInstance(type) as DialogueNode;
            node.TargetID = 0;
            node.TreeID = treeID;
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
        
        //TODO 类型转换支持
        public T GetConstant<T>(string variableName)
        {
            SharedVariable sharedVariable = this.Variables.FirstOrDefault(x => x.name == variableName);
            if (sharedVariable == null || sharedVariable.value == null) return default;
            try
            {
                
                T convertValue = (T)sharedVariable.value;
                if (typeof (T).IsValueType) return convertValue;
                
                T cloneValue = MongoHelper.Clone(convertValue);
                return cloneValue;
            }
            catch (Exception e)
            {
                Debug.LogError($"变量{variableName}转换失败!不能将{sharedVariable.value.GetType()}转换成{typeof (T)}\n"+e);
                return default;
            }
        }

#if UNITY_EDITOR
        public void Init()
        {
            treeName = "";
            root = null;
            nodes.Clear();
            blockDatas.Clear();
            NodeLinkDatas.Clear();
            targets.Clear();
            Variables.Clear();
        }

        public void Export()
        {
            var folderPath = DialogueSettings.GetSettings().ExportPath;
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            var fileName = $"{folderPath}/{treeName}.json";
            using FileStream json = new(fileName, FileMode.Create);
            using StreamWriter sw = new(json);
            sw.WriteLine(MongoHelper.ToJson(TranformToBsonDocument()));
        }

        private BsonDocument TranformToBsonDocument()
        {
            BsonDocument bsonDocument = new();
            Dictionary<string, BsonValue> tmpDic = new();

            //1. 节点数据
            bsonDocument.Add("treeID", treeID);
            
            var nodeDoc = new BsonDocument();
            bsonDocument.Add("targets", nodeDoc);
            targets.ForEach(kv =>
            {
                DialogueNode node = kv.Value;
                //1. 移除编辑器相关的属性
                var subDoc = node.ToBsonDocument();
                subDoc.Remove("Guid");
                subDoc.Remove("position");

                //2. 节点的唯一全局唯一表示ID
                subDoc.Remove("TreeID");
                subDoc.Remove("TargetID");
                subDoc.Add("ID", new BsonInt64(node.GetID()));

                //3. 去掉scripts中的注释
                subDoc.Remove("Script");
                string[] lines = node.Script.Split('\n');
                string result = string.Join("\n", lines.Select(line =>
                {
                    int index = line.IndexOf('#');
                    return index >= 0? line.Substring(0, index).Trim() : line.Trim();
                }).Where(filteredLine => !string.IsNullOrWhiteSpace(filteredLine)));
                subDoc.Add("Script", result);

                //4. 本地化
                subDoc.Remove("LocalizationGroups");
                BsonDocument contentDoc = new();
                Language[] languages = (Language[])Enum.GetValues(typeof (Language));
                languages.ForEach(lan => { contentDoc.Add(new BsonElement(lan.ToString(), node.GetContent(lan))); });

                subDoc.Add("content", contentDoc);
                tmpDic.Add(kv.Key.ToString(), subDoc);
            });
            tmpDic.ForEach(kv => { nodeDoc.Add(kv.Key, kv.Value); });
            //反序列化时需要知道节点个数 (Document索引0 --- length-1 为节点 第length个位置才是长度)
            nodeDoc.Add(new BsonElement("Length", targets.Count));

            //2. 变量
            var variableDoc = new BsonDocument();
            bsonDocument.Add("variables", variableDoc);
            for (int i = 0; i < Variables.Count; i++)
            {
                var subDoc = Variables[i].ToBsonDocument();
                variableDoc.Add(i.ToString(), subDoc);
            }

            variableDoc.Add("Length", Variables.Count);
            return bsonDocument;
        }
#endif
    }
}