using System;
using System.Collections.Generic;
using System.Reflection;
using ET;
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
    }
}