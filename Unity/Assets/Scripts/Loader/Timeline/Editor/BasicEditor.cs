using UnityEditor;
using UnityEngine;

namespace Timeline.Editor
{
    [CustomEditor(typeof (Basic), true)]
    public class BasicEditor: UnityEditor.Editor
    {
        private Basic m_script;
        private SerializedProperty scriptProp;
        private float width_view;
        protected GUILayoutOption width_whole;
        private GUILayoutOption width_half;

        private void OnEnable()
        {
            m_script = (Basic)target;
            scriptProp = serializedObject.FindProperty("m_Script");
        }

        public override void OnInspectorGUI()
        {
            width_whole = GUILayout.Width(width_view);
            width_half = GUILayout.Width(width_view / 2);

            serializedObject.Update();
            GUI.enabled = false;
            EditorGUILayout.PropertyField(scriptProp);
            GUI.enabled = true;
            EditorGUILayout.Space();

            m_script.m_clip = (UnityEngine.AnimationClip)EditorGUILayout.ObjectField("基础动画", m_script.m_clip, typeof (UnityEngine.AnimationClip), true);
            if (m_script.m_clip != null)
            {
                BasicExpressionInfo info = m_script.m_info;
                if (GUILayout.Button("读取动画数据"))
                {
                    foreach (var binding in AnimationUtility.GetCurveBindings(m_script.m_clip))
                    {
                        AnimationCurve curve = AnimationUtility.GetEditorCurve(m_script.m_clip, binding); //获取animationcurve
                        string _name = binding.propertyName;

                        if (!m_script.m_baseExpressionName.Contains(_name))
                        {
                            m_script.m_baseExpressionName.Add(_name);
                            m_script.m_baseExpressionValue.Add(0);
                            FloatAttr floatAttr = new() { m_value = new float[curve.keys.Length] };

                            info.m_baseExpressionFloat.Add(floatAttr);
                            StringArr stringArr = new() { m_value = new string[curve.keys.Length] };

                            info.m_baseExpressionString.Add(stringArr);
                            info.m_baseExpressionIndex.Add(0);
                        }

                        int index = m_script.m_baseExpressionName.IndexOf(_name);
                        for (int i = 0; i < curve.length; i++)
                        {
                            info.m_baseExpressionFloat[index].m_value[i] = curve[i].value;
                            info.m_baseExpressionString[index].m_value[i] = curve[i].time + " --- " + curve[i].value;
                        }
                    }
                }

                bool isWidth = false;
                info.m_foldoutAnimation = EditorGUILayout.BeginFoldoutHeaderGroup(info.m_foldoutAnimation, "BlendShape（动画：帧数 --- 数值）");
                if (info.m_foldoutAnimation)
                {
                    isWidth = true;
                }

                if (info.m_foldoutAnimation)
                {
                    string[] names = m_script.m_baseExpressionName.ToArray();
                    for (int i = 0; i < names.Length; i++)
                    {
                        info.m_baseExpressionIndex[i] =
                                EditorGUILayout.Popup(names[i], info.m_baseExpressionIndex[i], info.m_baseExpressionString[i].m_value);
                    }
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();
                if (GUILayout.Button("重置所有数据为动画数据"))
                {
                    string[] names = m_script.m_baseExpressionName.ToArray();
                    for (int i = 0; i < names.Length; i++)
                    {
                        m_script.m_baseExpressionValue[i] = info.m_baseExpressionFloat[i].m_value[info.m_baseExpressionIndex[i]];
                    }
                }

                info.m_foldoutPredefine = EditorGUILayout.BeginFoldoutHeaderGroup(info.m_foldoutPredefine, "BlendShape（预设值）");
                if (!isWidth && info.m_foldoutPredefine)
                {
                    isWidth = true;
                }

                if (info.m_foldoutPredefine)
                {
                    string[] names = m_script.m_baseExpressionName.ToArray();
                    for (int i = 0; i < names.Length; i++)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(names[i], width_half);
                        if (GUILayout.Button("重置为动画数据", width_half))
                        {
                            m_script.m_baseExpressionValue[i] = info.m_baseExpressionFloat[i].m_value[info.m_baseExpressionIndex[i]];
                        }

                        GUILayout.EndHorizontal();
                        m_script.m_baseExpressionValue[i] = EditorGUILayout.Slider(m_script.m_baseExpressionValue[i], 0, 100);
                    }
                }

                if (isWidth)
                {
                    width_view = EditorGUIUtility.currentViewWidth - 39;
                }
                else
                {
                    width_view = EditorGUIUtility.currentViewWidth - 30;
                }
            }

            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}