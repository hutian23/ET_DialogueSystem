﻿using MongoDB.Bson;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    [CustomEditor(typeof (DialogueViewComponent))]
    public class DialogueViewComponentEditor: OdinEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("重载对话树"))
            {
                if (this.target is not DialogueViewComponent component || component.tree == null) return;
                component.cloneTree = component.tree.DeepClone();
                if (Application.isPlaying)
                    EventSystem.Instance.Invoke(new ViewComponentReloadCallback()
                    {
                        instanceId = component.instanceId, ReloadType = ViewReloadType.Reload
                    });
            }

            if (GUILayout.Button("对话树视图"))
            {
                if (target is not DialogueViewComponent component || component.cloneTree == null) return;
                DialogueEditor.OpenWindow(component.cloneTree, component);
            }

            if (GUILayout.Button("保存对话树"))
            {
                if (target is not DialogueViewComponent component) return;
                if (component.tree == null)
                {
                    Debug.LogError("引用为空!");
                    return;
                }

                if (component.cloneTree == null)
                {
                    Debug.LogError("克隆树为空!");
                    return;
                }

                //有一些属性 eg. Status 不希望被保存 需要添加 BsonIgnore
                var cloneTree2 = MongoHelper.Clone(component.cloneTree);
                EditorUtility.CopySerialized(cloneTree2, component.tree);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("测试变量"))
            {
                if (this.target is not DialogueViewComponent component) return;
                component.cloneTree.Variables.ForEach(v =>
                {
                    Debug.Log(MongoHelper.ToJson(v));
                    Debug.Log(MongoHelper.Clone(v).ToJson());
                });
            }

            if (GUILayout.Button("测试keyframe"))
            {
                Keyframe keyframe = new();
                Debug.Log(keyframe.ToJson());
                Keyframe cloneFrame = MongoHelper.Clone(keyframe);
                Debug.Log(cloneFrame.ToJson());
                Debug.Log(keyframe.ToBsonDocument());
                Debug.Log(keyframe.ToBson());
            }
            
            // if (GUILayout.Button("序列化克隆树(测试)"))
            // {
            //     if (target is not DialogueViewComponent component) return;
            //     byte[] bytes = MongoHelper.Serialize(component.cloneTree);
            //     DialogueTree tree = MongoHelper.Deserialize<DialogueTree>(bytes);
            //     Debug.LogWarning(tree.ToJson());
            // }
        }
    }
}