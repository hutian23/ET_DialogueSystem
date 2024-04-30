using System;
using System.Collections.Generic;
using System.Reflection;
using ET;

namespace Timeline
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
    }
}