using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

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
                tree.treeId = 0;
                tree.treeName = "";
                tree.root = null;
                tree.nodes.Clear();
                tree.blockDatas.Clear();
                tree.targets.Clear();
            }

            if (GUILayout.Button("测试序列化"))
            {
                Debug.Log(MongoHelper.ToJson(this.target as DialogueTree));
            }
        }
    }
}