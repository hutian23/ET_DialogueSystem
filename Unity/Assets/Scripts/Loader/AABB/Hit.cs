using System;
using UnityEngine;

namespace AABB
{
    public class Hit: IHit
    {
        public Hit()
        {
            Normal = Vector2.Zero;
            Amount = 1.0f;
        }

        public IBox Box { get; }
        public Vector2 Normal { get; }
        public float Amount { get; }
        public Vector2 Position { get; }
        public float Romaining { get; }

        public bool IsNearest(IHit than, Vector2 from)
        {
            return false;
        }

        #region Function

        public static IHit Resolve(RectangleF origin, RectangleF destination, IBox other)
        {
            // var result = Resolve(origin, destination, other.Bounds);
            return null;
        }

        public static IHit Resolve(Vector3 origin, Vector2 destination, IBox other)
        {
            return null;
        }

        public static Hit Resolve(RectangleF origin, Vector2 destination, RectangleF other)
        {
            return null;
        }

        public static Hit Resolve(Vector2 origin, Vector2 destination, RectangleF other)
        {
            return null;
        }

        public static IHit Resolve(Vector2 point, IBox other)
        {
            return null;
        }

        #endregion

        private static Tuple<Vector2, Vector2> PushOutside(Vector2 origin, RectangleF other)
        {
            var position = origin;
            var normal = Vector2.Zero;

            var top = origin.Y - other.Top;
            var bottom = other.Bottom - origin.Y;
            var left = origin.X - other.Left;
            var right = other.Right - origin.X;

            var min = Math.Min(top, Math.Min(bottom, Math.Min(right, left)));

            // if(Math.Abs(min - top) < Constants.k)
            return null;
        }
    }
}