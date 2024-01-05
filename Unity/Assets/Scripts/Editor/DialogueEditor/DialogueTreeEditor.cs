using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    [CustomEditor(typeof(DialogueTree))]
    public class DialogueTreeEditor : OdinEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("打开对话树"))
            {   
                if(this.target is not DialogueTree tree) return;
                DialogueEditor.OpenWindow(tree);
            }
        }
    }
}