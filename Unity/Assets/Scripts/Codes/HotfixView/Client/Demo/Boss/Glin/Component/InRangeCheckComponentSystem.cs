using System.Numerics;
using Box2DSharp.Common;

namespace ET.Client
{
    [FriendOf(typeof(InRangeCheckComponent))]
    public static class InRangeCheckComponentSystem
    {
        public class InRangeCheckComponentDestroySystem : DestroySystem<InRangeCheckComponent>
        {
            protected override void Destroy(InRangeCheckComponent self)
            {
                self.inRange = false;
                self.center = UnityEngine.Vector2.zero;
                self.radius = 0f;
                self.inRangeCallbackIndex = 0;
            }
        }

        [FriendOf(typeof(b2Body))]
        [FriendOf(typeof(BBParser))]
        public class InRangeCheckComponentPostStepSystem : PostStepSystem<InRangeCheckComponent>
        {
            protected override void PosStepUpdate(InRangeCheckComponent self)
            {
                Unit player = BBUnitHelper.GetPlayer(self.ClientScene());
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
                b2Body bodyB = b2WorldManager.Instance.GetBody(unit.InstanceId);
                BBParser parser = self.GetParent<BBParser>();

                Vector2 center = bodyB.GetPosition() + self.center.ToVector2();
                Vector2 targetPoint = bodyA.GetPosition();

                self.inRange = Vector2.Distance(center, targetPoint) <= self.radius;
                // 触发回调
                if (self.inRange && self.inRangeCallbackIndex != 0)
                {
                    parser.Invoke(self.inRangeCallbackIndex, parser.CancellationToken).Coroutine();
                }
            }
        }

        public class InRangeCheckComponentGizmosUpdateSystem : GizmosUpdateSystem<InRangeCheckComponent>
        {
            protected override void GizmosUpdate(InRangeCheckComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                Vector2 center = b2Body.GetPosition() + self.center.ToVector2();
                b2WorldManager.Instance.DrawPoint(center, 8f, Color.White);
                b2WorldManager.Instance.DrawCircle(center, self.radius, Color.White);
            }
        }

        public static bool InRange(this InRangeCheckComponent self)
        {
            return self.inRange;
        }
    }
}