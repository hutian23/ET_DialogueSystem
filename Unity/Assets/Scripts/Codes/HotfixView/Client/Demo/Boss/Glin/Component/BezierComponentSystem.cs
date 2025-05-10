using UnityEngine;
using Color = Box2DSharp.Common.Color;

namespace ET.Client
{
    [FriendOf(typeof(BezierComponent))]
    public static class BezierComponentSystem
    {
        public class BezierComponentDestroySystem : DestroySystem<BezierComponent>
        {
            protected override void Destroy(BezierComponent self)
            {
                self.percent = 0f;
                self.speed = 0f;
                self.startPoint = Vector2.zero;
                self.midPoint = Vector2.zero;
                self.endPoint = Vector2.zero;
            }
        }

        public class BezierComponentPreStepSystem : PreStepSystem<BezierComponent>
        {
            protected override void PreStepUpdate(BezierComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body b2body = b2WorldManager.Instance.GetBody(unit.InstanceId);
                if (self.percent >= 1f)
                {
                    self.Dispose();
                    return;
                }
                
                // 计算百分比
                float percentSpeed = self.speed / (self.endPoint - self.startPoint).magnitude;
                self.percent += percentSpeed * (1 / 60f);
                if (self.percent > 1f) self.percent = 1;
                
                // 运动到哪个位置
                Vector2 ab = Vector2.Lerp(self.startPoint, self.midPoint, self.percent);
                Vector2 bc = Vector2.Lerp(self.midPoint, self.endPoint, self.percent);
                //当前帧速度 = 当前帧位移量 / 帧间间隔
                System.Numerics.Vector2 targetPos = Vector2.Lerp(ab, bc, self.percent).ToVector2();
                b2body.SetPosition(targetPos);
            }
        }
        
        public class BezierComponentGizmosSystem : GizmosUpdateSystem<BezierComponent>
        {
            protected override void GizmosUpdate(BezierComponent self)
            {
                b2WorldManager.Instance.DrawPoint(self.startPoint.ToVector2(), 8f, Color.Green);
                b2WorldManager.Instance.DrawPoint(self.midPoint.ToVector2(), 8f, Color.Blue);
                b2WorldManager.Instance.DrawPoint(self.endPoint.ToVector2(), 8f, Color.Red);
            }
        }
    }
}