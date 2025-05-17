using System.Numerics;
using Box2DSharp.Common;

namespace ET.Client
{
    public static class ZakoChaseComponentSystem
    {
        public class ZakoChaseComponentDestroySystem : DestroySystem<ZakoChaseComponent>
        {
            protected override void Destroy(ZakoChaseComponent self)
            {
                self.distance = 0f;
                self.velocity = 0f;
            }
        }
        
        public class ZakoChaseComponentGizmosUpdateSystem : GizmosUpdateSystem<ZakoChaseComponent>
        {
            protected override void GizmosUpdate(ZakoChaseComponent self)
            {
                Unit unitA = BBUnitHelper.GetPlayer(self.ClientScene());
                Unit unitB = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body bodyA = b2WorldManager.Instance.GetBody(unitA.InstanceId);
                b2Body bodyB = b2WorldManager.Instance.GetBody(unitB.InstanceId);
                
                b2WorldManager.Instance.DrawPoint(bodyA.GetPosition(), 8f, Color.White);
                b2WorldManager.Instance.DrawPoint(bodyB.GetPosition(), 8f, Color.White);
                
                float _distanceSqrt = Vector2.DistanceSquared(bodyA.GetPosition(), bodyB.GetPosition());
                float distanceSqrt = self.distance * self.distance;
                b2WorldManager.Instance.DrawSegment(bodyA.GetPosition(), bodyB.GetPosition(), _distanceSqrt >= distanceSqrt ? Color.Red : Color.Green);
            }
        }
    }
}