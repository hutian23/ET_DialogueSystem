using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public static class TimelineEditorUtility
    {
        public static Dictionary<Type, MonoScript> TrackScriptMap = new();
        public static Dictionary<Type, MonoScript> ClipScriptMap = new();
        public static Dictionary<Type, MonoScript> ClipInspectorViewScriptMap = new();

        static TimelineEditorUtility()
        {
            BuildScriptCached();
        }
        
        private static void BuildScriptCached()
        {
            foreach (var trackType in TypeCache.GetTypesDerivedFrom<Track>())
            {
                var trackScriptAsset = FindScriptFromClassName(trackType);
                if (trackScriptAsset != null)
                {
                    TrackScriptMap[trackType] = trackScriptAsset;
                }
            }

            foreach (var clipType in TypeCache.GetTypesDerivedFrom<Clip>())
            {
                var clipScriptAsset = FindScriptFromClassName(clipType);
                if (clipScriptAsset != null)
                {
                    ClipScriptMap[clipType] = clipScriptAsset;
                }
            }

            foreach (var clipInspectorViewType in TypeCache.GetTypesDerivedFrom<TimelineClipInspectorView>())
            {
                var clipInspectorViewScriptAsset = FindScriptFromClassName(clipInspectorViewType);
                if (clipInspectorViewScriptAsset != null)
                {
                    ClipInspectorViewScriptMap[clipInspectorViewType] = clipInspectorViewScriptAsset;
                }
            }
        }

        private static MonoScript FindScriptFromClassName(Type type)
        {
            var scriptGUIDs = ScriptGuidAttribute.Guids(type);
            foreach (var scriptGUID in scriptGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(scriptGUID);
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);

                if (script != null)
                {
                    return script;
                }
            }

            scriptGUIDs = AssetDatabase.FindAssets($"t:script {type.Name}");
            foreach (var scriptGUID in scriptGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(scriptGUID);
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
                //忽略大小写
                if (script != null && string.Equals(type.Name, Path.GetFileNameWithoutExtension(assetPath), StringComparison.OrdinalIgnoreCase))
                {
                    return script;
                }
            }
            return null;
        }

        public class MonoScriptInfo
        {
            public MonoScript Mono;
            public int lineNumber;
            public int ColumnNumber;

            public MonoScriptInfo(MonoScript mono, int ln = 0, int cn = 0)
            {
                Mono = mono;
                lineNumber = ln;
                ColumnNumber = cn;
            }

            public MonoScriptInfo()
            {
                
            }
        }

        public static void SelectTrackScript<T>(this T target) where T : Track
        {
            if (TrackScriptMap.TryGetValue(target.GetType(), out MonoScript monoScript))
            {
                Selection.activeObject = monoScript;
            }
        }

        public static void OpenTrackScript<T>(this T target) where T : Track
        {
            if (TrackScriptMap.TryGetValue(target.GetType(), out MonoScript monoScript))
            {
                AssetDatabase.OpenAsset(monoScript.GetInstanceID());
            }
        }

        public static bool ShowIf(this MemberInfo memberInfo, object target)
        {
            if (memberInfo.GetCustomAttributes<ShowIfAttribute>() is ShowIfAttribute showIfAttribute && !showIfAttribute.Show(target))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool HideIf(this MemberInfo memberInfo, object target)
        {
            if (memberInfo.GetCustomAttribute<HideIfAttribute>() is HideIfAttribute hideIfAttribute && hideIfAttribute.Hide(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public static bool ReadOnly(this MemberInfo memberInfo, object target)
        {
            if (memberInfo.GetCustomAttributes<ReadOnlyAttribute>() is ReadOnlyAttribute readOnlyAttribute && readOnlyAttribute.ReadOnly(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Group(this MemberInfo memberInfo, VisualElement content, float index, ref List<VisualElement> visualElements, ref Dictionary<string, (VisualElement, List<VisualElement>)> groupMap)
        {
            string groupName = string.Empty;
            if (memberInfo.GetCustomAttributes<HorizontalGroupAttribute>() is HorizontalGroupAttribute horizontalGroupAttribute)
            {
                groupName = horizontalGroupAttribute.GroupName;
            }

            Splitline splitline = memberInfo.GetCustomAttribute<Splitline>();
            if (!string.IsNullOrEmpty(groupName))
            {
                VisualElement group = new();
                group.style.flexDirection = FlexDirection.Row;
                group.name = index * 10 + visualElements.Count.ToString();
                visualElements.Add(group);
                groupMap.Add(groupName,(group,new List<VisualElement>()));

                if (splitline != null)
                {
                    group.style.paddingTop = splitline.Space;
                    group.style.borderTopColor = new Color(88, 88, 88, 255) / 255;
                    group.style.borderTopWidth = 1;
                }
                groupMap[groupName].Item2.Add(content);
            }
            else
            {
                visualElements.Add(content);
                if (splitline != null)
                {
                    content.style.paddingTop = splitline.Space;
                    content.style.borderTopColor = new Color(88, 88, 88, 255) / 255;
                    content.style.borderTopWidth = 1;
                }
            }
        }
    }
}