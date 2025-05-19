using System;
using System.Numerics;
using Box2DSharp.Common;

namespace ET.Client
{
    [FriendOf(typeof(PatrolComponent))]
    public static class PatrolComponentSystem
    {
        public class PatrolComponentDestroySystem : DestroySystem<PatrolComponent>
        {
            protected override void Destroy(PatrolComponent self)
            {
                self.minX = 0;
                self.maxX = 0;
            }
        }

        public class PatrolComponentGizmosUpdateSystem : GizmosUpdateSystem<PatrolComponent>
        {
            protected override void GizmosUpdate(PatrolComponent self)
            {
                Unit unit = self.GetParent<Unit>();
                b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                b2WorldManager.Instance.DrawPoint(new Vector2(self.minX, body.GetPosition().Y), 8f, Color.Red);
                b2WorldManager.Instance.DrawPoint(new Vector2(self.maxX, body.GetPosition().Y), 8f, Color.Red);
            }
        }

        public static bool HasReached(this PatrolComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            float distanceA = Math.Abs(body.GetPosition().X - self.minX);
            float distanceB = Math.Abs(body.GetPosition().X - self.maxX);

            return distanceA <= 0.1f || distanceB <= 0.1f;
        }
    }
}