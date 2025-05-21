using System;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    public static class Box2DHelper
    {
        public static float Vector2ToAngle(this Vector2 vector)
        {
            // 使用Atan2计算弧度，然后转换为度
            float angleRadians = MathF.Atan2(vector.Y, vector.X);
            float angleDegrees = angleRadians * (180f / MathF.PI);
        
            // 将角度规范化为0-360范围
            if (angleDegrees < 0)
            {
                angleDegrees += 360f;
            }
        
            return angleDegrees;
        }
        
        // 角度（弧度）→ Vector2 单位向量
        private static Vector2 AngleToVector(float angleInRadians)
        {
            float x = MathF.Cos(angleInRadians);
            float y = MathF.Sin(angleInRadians);
            return new Vector2(x, y);
        }

        // 角度（度数）→ Vector2 单位向量
        public static Vector2 DegreesToVector(float angleInDegrees)
        {
            float radians = angleInDegrees * MathF.PI / 180f;
            return AngleToVector(radians);
        }
        
        public static float Vector2ToRadians(this Vector2 vector)
        {
            return MathF.Atan2(vector.Y, vector.X);
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }

        public static Vector2 GetReflection(Vector2 inVector, Vector2 normal)
        {
            //确保法向量归一化
            normal = Vector2.Normalize(normal);
            
            //计算入射向量在法向量上的投影
            float dotProduct = Vector2.Dot(inVector, normal);
            
            //计算反射向量： R = I - 2 * (I·N) * N
            return inVector - 2 * dotProduct * normal;
        }

        public static bool IsPointInBox(Vector2 point, Vector2 boxCenter, Vector2 boxSize)
        {
            Vector2 halfSize = boxSize / 2f;
            Vector2 min = boxCenter - halfSize;
            Vector2 max = boxCenter + halfSize;
        
            return point.X >= min.X && point.X <= max.X &&
                    point.Y >= min.Y && point.Y <= max.Y;
        }
    }
}