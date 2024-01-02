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
            if (GUILayout.Button("打开对话树"))
            {
                DialogueEditor.OpenWindow(this.tree);
            }
        }
    }
}