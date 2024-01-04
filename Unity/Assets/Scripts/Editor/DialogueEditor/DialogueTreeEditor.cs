using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    [CustomEditor(typeof (DialogueTree))]
    public class DialogueTreeEditor: Editor
    {
        public DialogueTree tree;

        public void OnEnable()
        {
            this.tree = (DialogueTree)this.target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // GUILayout.Space(10);
            // this.tree.treeId = EditorGUILayout.IntField("对话树ID: ", this.tree.treeId);
            // this.tree.treeName = EditorGUILayout.TextField("对话树名称: ", this.tree.treeName);
            // GUILayout.Space(10);
            // if (GUILayout.Button("打开对话树"))
            // {
            //     DialogueEditor.OpenWindow(this.tree);
            // }
        }
    }
}