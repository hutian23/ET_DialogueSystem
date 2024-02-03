using MongoDB.Bson;
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
            DialogueViewComponent component = target as DialogueViewComponent;
            if (component == null) return;
            component.OpenWindow ??= () => { DialogueEditor.OpenWindow(component); };

            // if (GUILayout.Button("测试keyframe"))
            // {
            //     var keyFrame = new Keyframe(){tangentMode = 233};
            //     var cloneFrame = MongoHelper.Clone(keyFrame);
            //     Debug.Log(keyFrame.ToJson());
            //     Debug.Log(cloneFrame.ToJson());
            // }
            
            // if (GUILayout.Button("测试变量"))
            // {
            //     component.cloneTree.Variables.ForEach(v =>
            //     {
            //         Debug.Log(MongoHelper.ToJson(v));
            //         Debug.Log(MongoHelper.Clone(v).ToJson());
            //     });
            // }

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