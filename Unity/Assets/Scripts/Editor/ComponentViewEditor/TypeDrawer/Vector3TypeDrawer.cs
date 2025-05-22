using System;
using Box2DSharp.Testbed.Unity.Inspection;
using UnityEditor;

namespace ET
{
    public class Vector3TypeDrawer: ITypeDrawer
    {
        public bool HandlesType(Type type)
        {
            return type == typeof (System.Numerics.Vector3);
        }

        public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
        {
            System.Numerics.Vector3 vector3 = (System.Numerics.Vector3)value;
            UnityEngine.Vector3 _vector3 = new UnityEngine.Vector3(vector3.X, vector3.Y, vector3.Z);
            return EditorGUILayout.Vector3Field(memberName, _vector3).ToVector3();
        }
    }
}