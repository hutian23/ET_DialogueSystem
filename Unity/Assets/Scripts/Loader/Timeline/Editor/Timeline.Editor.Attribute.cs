using System;
using System.Reflection;
using UnityEngine;

namespace Timeline.Editor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute: Attribute
    {
        public string ShowIfTarget;

        public ShowIfAttribute(string showIfTarget = null)
        {
            ShowIfTarget = showIfTarget;
        }

        public bool Show(object target)
        {
            FieldInfo fieldInfo = target.GetField(ShowIfTarget);
            if (fieldInfo != null)
            {
                return (bool)fieldInfo.GetValue(target);
            }

            PropertyInfo propertyInfo = target.GetProperty(ShowIfTarget);
            if (propertyInfo != null)
            {
                return (bool)propertyInfo.GetValue(target);
            }

            MethodInfo method = target.GetMethod(ShowIfTarget);
            if (method != null)
            {
                return (bool)method.Invoke(target, null);
            }

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HideIfAttribute: Attribute
    {
        public string HideIfTarget;

        public HideIfAttribute(string hideIfTarget = null)
        {
            HideIfTarget = hideIfTarget;
        }

        public bool Hide(object target)
        {
            FieldInfo fieldInfo = target.GetField(HideIfTarget);
            if (fieldInfo != null)
            {
                return (bool)fieldInfo.GetValue(target);
            }

            PropertyInfo propertyInfo = target.GetProperty(HideIfTarget);
            if (propertyInfo != null)
            {
                return (bool)propertyInfo.GetValue(target);
            }

            MethodInfo method = target.GetMethod(HideIfTarget);
            if (method != null)
            {
                return (bool)method.Invoke(target, null);
            }

            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute: Attribute
    {
        public string ReadOnlyTarget;

        public ReadOnlyAttribute(string readOnlyTarget = null)
        {
            ReadOnlyTarget = readOnlyTarget;
        }

        public bool ReadOnly(object target)
        {
            FieldInfo fieldInfo = target.GetField(ReadOnlyTarget);
            if (fieldInfo != null)
            {
                return (bool)fieldInfo.GetValue(target);
            }

            PropertyInfo propertyInfo = target.GetProperty(ReadOnlyTarget);
            if (propertyInfo != null)
            {
                return (bool)propertyInfo.GetValue(target);
            }

            MethodInfo method = target.GetMethod(ReadOnlyTarget);
            if (method != null)
            {
                return (bool)method.Invoke(target, null);
            }

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class GroupAttribute: Attribute
    {
        public string GroupName;

        public GroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HorizontalGroupAttribute: Attribute
    {
        public string GroupName;

        public HorizontalGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class ScriptGuidAttribute: Attribute
    {
        public string ScriptGuid;

        public ScriptGuidAttribute(string scriptGuid)
        {
            ScriptGuid = scriptGuid;
        }

        public static string[] Guids(Type type)
        {
            var scriptGuidAttributes = type.GetAttributes<ScriptGuidAttribute>();
            string[] guids = new string[scriptGuidAttributes.Length];
            for (int i = 0; i < scriptGuidAttributes.Length; i++)
            {
                guids[i] = scriptGuidAttributes[i].ScriptGuid;
            }

            return guids;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class IconGuidAttribute: Attribute
    {
        public string IconGuid;

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

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ObjectFieldAttribute: PropertyAttribute
    {
        public string OnValueChangedCallback;
        public string BindPath;

        public ObjectFieldAttribute(string onValueChangedCallback, string bindPath = null)
        {
            OnValueChangedCallback = onValueChangedCallback;
            BindPath = bindPath;
        }
    }
}