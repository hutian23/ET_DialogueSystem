using System;
using Box2DSharp.Testbed.Unity.Inspection;
using UnityEditor;

namespace ET
{
    [TypeDrawer]
    public class Vector2TypeDrawer : ITypeDrawer
    {
        public bool HandlesType(Type type)
        {
            return type == typeof(System.Numerics.Vector2);
        }

        public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
        {
            System.Numerics.Vector2 vector2 = (System.Numerics.Vector2)value;
            UnityEngine.Vector2 _vector2 = new UnityEngine.Vector2(vector2.X, vector2.Y);
            return EditorGUILayout.Vector2Field(memberName, _vector2).ToVector2();
        }
    }
}