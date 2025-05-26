using Timeline;
using Timeline.Editor;
using UnityEditor;
using UnityEngine;

namespace ET
{
    [CustomEditor(typeof(TimelinePlayer))]
    public class TimelinePlayerEditor : Editor
    {
        private SerializedProperty m_PlayableGraph;

        private void OnEnable()
        {
            m_PlayableGraph = serializedObject.FindProperty("PlayableGraph");
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space(5);
            m_PlayableGraph.objectReferenceValue = EditorGUILayout.ObjectField("PlayableGraph", m_PlayableGraph.objectReferenceValue, typeof (BBPlayableGraph), false);
            EditorGUILayout.Space(5);
            
            // Button
            TimelinePlayer timelinePlayer = serializedObject.targetObject as TimelinePlayer;
            if (timelinePlayer.instanceId == 0)
            {
                if (GUILayout.Button("技能编辑器"))
                {
                    foreach (BBTimeline timeline in timelinePlayer.PlayableGraph.timelineDict.Values)
                    {
                        TimelineEditorWindow.OpenWindow(timelinePlayer, timeline);
                        return;
                    }
                }

                if (GUILayout.Button("清除运行时组件"))
                {
                    timelinePlayer.ClearTimelineGenerate();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}