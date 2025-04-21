using System.Numerics;

namespace ET.Client
{
    [FriendOf(typeof(RootMotionComponent))]
    public static class RootMotionComponentSystem
    {
        public class RootMotionComponentPreStepSystem : PreStepSystem<RootMotionComponent>
        {
            protected override void PreStepUpdate(RootMotionComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                B2Unit b2Unit = unit.GetComponent<B2Unit>();
                b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
                
                //(-1,1)是因为Timeline中角色的默认朝向为左
                Vector2 curVel = self.MotionVel * b2Unit.GetHertz() / 60f * new Vector2(body.GetFlip(), 1);
                body.SetVelocity(curVel);
                b2Unit.SetVelocity(curVel);
            }
        }

        public static void SetVel(this RootMotionComponent self, Vector2 motionVel)
        {
            self.MotionVel = motionVel;
        }
    }
}