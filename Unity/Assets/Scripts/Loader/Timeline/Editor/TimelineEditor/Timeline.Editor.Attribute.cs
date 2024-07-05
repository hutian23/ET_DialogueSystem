using System;
using Sirenix.Utilities;
using UnityEngine;

namespace Timeline.Editor
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class IconGuidAttribute: Attribute
    {
        private readonly string IconGuid;

        public IconGuidAttribute(string iconGuid)
        {
            IconGuid = iconGuid;
        }

        public static string Guid(Type type)
        {
            var iconGuidAttribute = type.GetAttribute<IconGuidAttribute>();
            if (iconGuidAttribute != null)
                return iconGuidAttribute.IconGuid;
            else
                return null;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ColorAttribute: Attribute
    {
        private readonly Color Color;

        public ColorAttribute(float r, float g, float b)
        {
            Color = new Color(r, g, b, 255);
        }

        public static Color GetColor(Type type)
        {
            var ColorAttribute = type.GetAttribute<ColorAttribute>();
            if (ColorAttribute != null)
            {
                return ColorAttribute.Color / 255;
            }

            return default;
        }
    }
}