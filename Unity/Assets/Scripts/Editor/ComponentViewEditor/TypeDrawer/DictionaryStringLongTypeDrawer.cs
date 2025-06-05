using System;
using System.Collections.Generic;
using UnityEditor;

namespace ET
{
    [TypeDrawer]
    public class DictionaryStringLongTypeDrawer: ITypeDrawer
    {
        public bool HandlesType(Type type)
        {
            return type == typeof (Dictionary<string, long>);
        }

        public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
        {
            Dictionary<string, long> dictionary = value as Dictionary<string, long>;
            
            EditorGUILayout.LabelField($"{memberName}:");
            foreach ((string k, long v) in dictionary)
            {
                // if (v == 0)
                // {
                //     continue;
                // }
                EditorGUILayout.LongField($"    {k} :", v);
            }
            
            return value;
        }
    }
}