using System;
using UnityEngine;

namespace AABB
{
    /// <summary>
    /// Describes a floating point 2D-rectangle
    /// </summary>
    [Serializable]
    public struct RectangleF: IEquatable<RectangleF>
    {
        public float X;

        public float Y;

        public float Width;

        public float Height;

        public static RectangleF Empty { get; } = new();

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public bool IsEmpty => Width.Equals(0) && Height.Equals(0) && X.Equals(0) && Y.Equals(0);

        /// <summary>
        /// The Top-Left coordinates of this < see cref = "RectangleF"/>
        /// </summary>
        public Vector2 Location
        {
            get
            {
                return new Vector2(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(Width, Height);
            }
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        public Vector2 Center => new Vector2(X + Width / 2f, Y + Height / 2);

        // internal string Debug

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF(Vector2 location, Vector2 size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.X;
            Height = size.Y;
        }

        #region operators

        public static bool operator ==(RectangleF a, RectangleF b)
        {
            const float epsilon = 0.00001f;
            return Math.Abs(a.X - b.X) < epsilon
                    && Math.Abs(a.Y - b.Y) < epsilon
                    && Math.Abs(a.Width - b.Width) < epsilon
                    && Math.Abs(a.Height - b.Height) < epsilon;
        }

        public static bool operator !=(RectangleF a, RectangleF b)
        {
            return !(a == b);
        }

        #endregion

        #region Contain

        public bool Contains(int x, int y)
        {
            return X <= x && x < X + Width && Y <= y && y < y + Height;
        }

        public bool Contains(float x, float y)
        {
            return X <= x && x < X + Width && Y <= y && y < y + Height;
        }

        public void Contains(ref Vector2 value, out bool result)
        {
            result = (X <= value.X) && (value.X < X + Width) && (Y <= value.Y) && (value.Y < Y + Height);
        }

        public bool Contains(RectangleF value)
        {
            return (X <= value.X) && (value.X + value.Width <= X + Width) && (Y <= value.Y) && (value.Y + value.Height <= Y + Height);
        }

        public void Contains(ref RectangleF value, out bool result)
        {
            result = (X <= value.X) && (value.X + value.Width <= X + Width) && (Y <= value.Y) && (value.Y + value.Height <= Y + Height);
        }

        #endregion

        public RectangleF GetBoundingRectangle()
        {
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is RectangleF f && this == f;
        }
        
        public bool Equals(RectangleF other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Width, Height);
        }

        /// <summary>
        /// Adjust the edges of this by specified horizontal and vertical amounts.
        /// </summary>
        /// <param name="horizontalAmount"></param>
        /// <param name="verticalAmount"></param>
        public void Inflate(int horizontalAmount, int verticalAmount)
        {
            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount * 2;
            Height += verticalAmount * 2;
        }

        #region Intersect

        /// <summary>
        /// Gets whether or not the other intersects(相交) with this rectangle
        /// </summary>
        public bool Intersects(RectangleF value)
        {
            return value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;
        }

        public void Intersects(ref RectangleF value, out bool result)
        {
            result = value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;
        }

        /// <summary>
        /// create a rectangleF that contains overlapping region of the two rectangles
        /// 求相交rectangle
        /// </summary>
        public static void Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            if (value1.Intersects(value2))
            {
                float rightSide = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
                float leftSide = Math.Max(value1.X, value2.X);
                float topSide = Math.Max(value1.Y, value2.Y);
                float bottomSide = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
                result = new RectangleF(leftSide, topSide, rightSide - leftSide, bottomSide - topSide);
            }
            else
            {
                result = new RectangleF(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Creates a new rectangleF that contains overlapping region of two other rectangles.
        /// 获得相交rectangle
        /// </summary>
        public static RectangleF Intersect(RectangleF value1, RectangleF value2)
        {
            RectangleF rectangle;
            Intersect(ref value1, ref value2, out rectangle);
            return rectangle;
        }

        #endregion

        /// <summary>
        /// Change the location of this rectangle
        /// </summary>
        public void Offset(int offsetX, int offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        /// <summary>
        /// Changes the location of this rectangle
        /// </summary>
        /// <param name="amount"></param>
        public void Offset(Vector3 amount)
        {
            X += amount.x;
            Y += amount.y;
        }

        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
        }

        /// <summary>
        /// Create a new rectangle that completely contians two rectangles.
        /// 并集
        /// </summary>
        public static RectangleF Union(RectangleF value1, RectangleF value2)
        {
            float x = Math.Min(value1.X, value2.X);
            float y = Math.Min(value1.Y, value2.Y);
            return new RectangleF(x, y, Math.Max(value1.Right, value2.Right) - x, Math.Max(value1.Bottom, value2.Bottom) - y);
        }

        /// <summary>
        /// Create a new rectangle from two points
        /// </summary>
        public static RectangleF FromPoints(Vector2 point0, Vector2 point1)
        {
            float x = Math.Min(point0.X, point1.X);
            float y = Math.Min(point0.Y, point1.Y);
            float width = Math.Abs(point0.X - point1.X);
            float height = Math.Abs(point0.Y - point1.Y);
            RectangleF rectangleF = new(x, y, width, height);
            return rectangleF;
        }

        /// <summary>
        /// Calculate the signed depth of intersection between two rectangles
        /// </summary>
        public Vector2 IntersectionDepth(RectangleF other)
        {
            //Calculate half sizes
            float thisHalfWidth = Width / 2.0f;
            float thisHalfWidthHeight = Height / 2.0f;
            float otherHalfWidth = other.Width / 2.0f;
            float otherHalfHeight = other.Height / 2.0f;

            //Calculate centers
            Vector2 centerA = new(Left + thisHalfWidth, Top + thisHalfWidthHeight);
            Vector2 centerB = new(other.Left + otherHalfWidth, other.Top + otherHalfHeight);

            //Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = thisHalfWidth + otherHalfWidth;
            float minDistanceY = thisHalfWidthHeight + otherHalfHeight;

            //If we are not instersecting at all,return (0,0)
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
            {
                return Vector2.Zero;
            }

            //Calculate and return intersection depth
            float depthX = distanceX > 0? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0? minDistanceY - distanceY : -minDistanceY - distanceY;

            return new Vector2(depthX, depthY);
        }
    }
}