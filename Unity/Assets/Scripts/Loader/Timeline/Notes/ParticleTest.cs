using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class ParticleTest: MonoBehaviour
    {
        public ParticleSystem particle;

        public AnimationClip clip;
        
        [Button("测试")]
        public void Test()
        {
            AnimationWindow animationWindow = EditorWindow.GetWindow<AnimationWindow>();
            animationWindow.animationClip = clip;
            animationWindow.Show();
        }
        
        // [Sirenix.OdinInspector.Button("测试")]
        // public void TestPlay()
        // {
        //     particle.PlayInEditor();
        // }
        //
        // [Range(0,10)]
        // [OnValueChanged("Test")]
        // public float Time;
        //
        // public void Test()
        // {
        //     particle.Simulate(Time);
        // }
        // public AnimationClip AnimationClip;
        //
        // [Sirenix.OdinInspector.Button("测试")]
        // public void Test()
        // {
        //     Debug.LogWarning(AnimationClip.frameRate);
        //     Debug.LogWarning(AnimationClip.legacy);
        //     GetSpriteKeyframes(AnimationClip);
        // }
        //
        // private void GetSpriteKeyframes(AnimationClip clip)
        // {
        //     // 获取所有绑定路径
        //     EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
        //
        //     foreach (var binding in bindings)
        //     {
        //         // 只处理SpriteRenderer的sprite属性
        //         if (binding.type == typeof (SpriteRenderer) && binding.propertyName == "m_Sprite")
        //         {
        //             // 获取对应的关键帧
        //             ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
        //
        //             foreach (var keyframe in keyframes)
        //             {
        //                 Sprite sprite = keyframe.value as Sprite;
        //                 Debug.Log($"Time: {Mathf.RoundToInt(keyframe.time * 60)}, Sprite: {sprite}");
        //             }
        //         }
        //     }
        // }
        //
        // public static AnimationCurve GetAnimCurve(AnimationClip clip, string path, string propName)
        // {
        //     EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);
        //     foreach (var b in bindings)
        //     {
        //         if (b.path == path)
        //         {
        //             //Debug.Log($"path:{b.path}, propName:{b.propertyName}, discrete:{b.isDiscreteCurve}, pptr:{b.isPPtrCurve}");
        //             if (b.propertyName == propName)
        //             {
        //                 var result = AnimationUtility.GetEditorCurve(clip, b);
        //                 return result;
        //             }
        //         }
        //     }
        //
        //     return null;
        // }
    }
}