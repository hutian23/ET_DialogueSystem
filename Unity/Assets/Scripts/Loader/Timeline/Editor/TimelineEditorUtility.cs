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
        private static Dictionary<Type, MonoScript> TrackScriptMap = new();
        private static Dictionary<Type, MonoScript> ClipScriptMap = new();
        private static Dictionary<Type, MonoScript> ClipInspectorViewScriptMap = new();

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