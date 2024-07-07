using System;
using System.Collections.Generic;
using System.Reflection;
using ET;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public static class BBTimelineEditorUtility
    {
        public static Dictionary<string, Type> BBTrackTypeDic = new();
        public static Dictionary<string, object> ParamsTypeDict = new();
        public static Dictionary<Type, Type> ParamsFieldDict = new();

        [UnityEditor.Callbacks.DidReloadScripts]
        public static void RegistParamsType()
        {
            //Add params
            ParamsTypeDict.Clear();
            ParamsTypeDict.Add("Int", 0);
            ParamsTypeDict.Add("Float", 0f);
            ParamsTypeDict.Add("Bool", false);
            ParamsTypeDict.Add("AnimationCurve", new AnimationCurve());
            ParamsTypeDict.Add("Gradient", new Gradient());

            //Init paramField
            ParamsFieldDict.Clear();
            ParamsFieldDict.Add(typeof (int), typeof (ParamResolver<IntegerField, int>));
            ParamsFieldDict.Add(typeof (float), typeof (ParamResolver<FloatField, float>));
            ParamsFieldDict.Add(typeof (bool), typeof (ParamResolver<Toggle, bool>));
            ParamsFieldDict.Add(typeof (string), typeof (ParamResolver<TextField, string>));
            ParamsFieldDict.Add(typeof (Vector2), typeof (ParamResolver<Vector2Field, Vector2>));
            ParamsFieldDict.Add(typeof (Vector3), typeof (ParamResolver<Vector3Field, Vector3>));
            ParamsFieldDict.Add(typeof (AnimationCurve), typeof (ParamResolver<CurveField, AnimationCurve>));
            ParamsFieldDict.Add(typeof (Gradient), typeof (ParamResolver<GradientField, Gradient>));
        }

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

        public static HashSet<int> GetAnimationKeyframes(AnimationClip clip)
        {
            HashSet<int> keyframeSet = new();
            // 获取所有绑定路径
            EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

            foreach (EditorCurveBinding binding in bindings)
            {
                ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                foreach (ObjectReferenceKeyframe keyframe in keyframes)
                {
                    keyframeSet.Add(Mathf.RoundToInt(keyframe.time * 60));
                }
            }

            return keyframeSet;
        }

        public static string GetFullPath(this GameObject go)
        {
            string path = "/" + go.name;
            Transform current = go.transform;

            while (current.parent != null)
            {
                current = current.parent;
                path = "/" + current.name + path;
            }

            return path;
        }

        public static void ForceScrollViewUpdate(this ScrollView view)
        {
            view.schedule.Execute(() =>
            {
                var fakeOldRect = Rect.zero;
                var fakeNewRect = view.layout;

                using var evt = GeometryChangedEvent.GetPooled(fakeOldRect, fakeNewRect);
                evt.target = view.contentContainer;
                view.contentContainer.SendEvent(evt);
            });
        }
    }
}