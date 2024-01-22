using System.IO;
using MongoDB.Bson;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using File = System.IO.File;

namespace ET.Client
{
    [CustomEditor(typeof (DialogueTree))]
    public class DialogueTreeEditor: OdinEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("打开对话树"))
            {
                if (this.target is not DialogueTree tree) return;
                DialogueEditor.OpenWindow(tree);
            }

            if (GUILayout.Button("重置对话树"))
            {
                if (this.target is not DialogueTree tree) return;
                tree.treeID = 0;
                tree.treeName = "";
                tree.root = null;
                tree.nodes.Clear();
                tree.blockDatas.Clear();
                tree.NodeLinkDatas.Clear();
                tree.targets.Clear();
            }

            if (GUILayout.Button("测试序列化"))
            {
                var tree = target as DialogueTree;
                tree.Export();
            }

            if (GUILayout.Button("测试反序列化"))
            {
                var tree = target as DialogueTree;
                var file = Path.Combine(DialogueSettings.GetSettings().ExportPath, $"{tree.treeName}.json");

                string jsonContent = File.ReadAllText(file);

                BsonDocument doc = MongoHelper.FromJson<BsonDocument>(jsonContent);
                var _v = doc["_v"];
                int length = (int)_v["Length"];
                for (int i = 0; i < length; i++)
                {
                    var nodeDoc = _v[i].ToBsonDocument();
                    DialogueNode node = MongoHelper.Deserialize<DialogueNode>(nodeDoc.ToBson());
                    node.FromID((long)nodeDoc.GetValue("ID"));

                    var contentDoc = nodeDoc.GetValue("content").ToBsonDocument();
                    node.text = (string)contentDoc[(int)Language.Chinese];
                    Debug.Log(node.ToJson() + "  " + node.text);
                }
            }
        }
    }
}