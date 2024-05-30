using System;
using System.Collections.Generic;
using System.Reflection;
using ET;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class BBTimelineEditorUtility
    {
        public static Dictionary<string, Type> BBTrackTypeDic = new();

        [UnityEditor.Callbacks.DidReloadScripts]
        public static void RegistBBTrack()
        {
            BBTrackTypeDic.Clear();
            var types = AssemblyHelper.GetAssemblyTypes(typeof (BBTimelineEditorUtility).Assembly);
            foreach (var type in types.Values)
            {
                if (type.IsGenericType || type.IsAbstract) continue;
                BBTrackAttribute attr = type.GetCustomAttribute<BBTrackAttribute>();
                if (attr == null) continue;
                BBTrackTypeDic.TryAdd(attr.TrackName, type);
            }
        }

        public static void DrawDiamond(Painter2D paint2D, float pos)
        {
            paint2D.BeginPath();
            paint2D.fillColor = Color.white;
            paint2D.MoveTo(new Vector2(pos - 4, 8));
            paint2D.LineTo(new Vector2(pos, 3));
            paint2D.LineTo(new Vector2(pos + 4, 8));
            paint2D.LineTo(new Vector2(pos, 13));
            paint2D.ClosePath();
            paint2D.Fill(FillRule.OddEven);
        }

        public static HashSet<int> GetAnimationKeyframes(UnityEngine.AnimationClip clip)
        {
            HashSet<int> keyframeSet = new();
            // 获取所有绑定路径
            EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

            foreach (var binding in bindings)
            {
                ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                foreach (var keyframe in keyframes)
                {
                    keyframeSet.Add(Mathf.RoundToInt(keyframe.time * 60));
                }
                // // 只处理SpriteRenderer的sprite属性
                // if (binding.type == typeof (SpriteRenderer) && binding.propertyName == "m_Sprite")
                // {
                //     // 获取对应的关键帧
                //     ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                //
                //     foreach (var keyframe in keyframes)
                //     {
                //         Sprite sprite = keyframe.value as Sprite;
                //         Debug.Log($"Time: {Mathf.RoundToInt(keyframe.time * 60)}, Sprite: {sprite}");
                //     }
                // }
            }
            return keyframeSet;
        }
    }
}