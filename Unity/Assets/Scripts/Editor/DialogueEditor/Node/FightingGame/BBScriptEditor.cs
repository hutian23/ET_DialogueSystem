using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    public class BBScriptEditor: EditorWindow
    {
        private BBNode target;
        Vector2 scroll;

        public static void Init(BBNode node)
        {
            BBScriptEditor editor = (BBScriptEditor)GetWindow(typeof (BBScriptEditor), true, "BBScriptEditor");
            editor.target = node;
            editor.Show();
        }

        void OnGUI()
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            target.BBScript = EditorGUILayout.TextArea(target.BBScript, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }
    }
}