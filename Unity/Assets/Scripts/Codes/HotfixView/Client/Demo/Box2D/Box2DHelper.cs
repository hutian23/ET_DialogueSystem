using System;
using System.Numerics;

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

        public static float Vector2ToRadians(this Vector2 vector)
        {
            return MathF.Atan2(vector.Y, vector.X);
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