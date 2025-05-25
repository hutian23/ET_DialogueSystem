using System;
using System.Collections.Generic;
using UnityEditor;

namespace ET
{
    [TypeDrawer]
    public class HashSetStringTypeDrawer : ITypeDrawer
    {
        public bool HandlesType(Type type)
        {
            return type == typeof (HashSet<string>);
        }

        public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
        {
            HashSet<string> hashSet = value as HashSet<string>;

            EditorGUILayout.LabelField($"{memberName}:");
            foreach (string _value in hashSet)
            {
                EditorGUILayout.LabelField($"    {_value}");
            }
            EditorGUILayout.Space(5);
            
            return value;
        }
    }
}