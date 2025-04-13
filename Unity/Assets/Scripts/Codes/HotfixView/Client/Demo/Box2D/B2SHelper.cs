using System;
using System.Numerics;

namespace ET.Client
{
    public static class B2SHelper
    {
        public static bool IsVectorEqual(Vector2 a, Vector2 b, float epsilon = 0.01f)
        {
            return MathF.Abs(a.X - b.X) < epsilon && MathF.Abs(a.Y - b.Y) < epsilon;
        }
    }
}