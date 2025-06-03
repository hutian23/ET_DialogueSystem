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
                b2Body body = b2WorldManager.Instance.GetBody(self.GetParent<BBParser>().GetParent<Unit>().InstanceId);
                
                //(-1,1)是因为Timeline中角色的默认朝向为左
                body.SetLinearVelocity(-self.MotionVel);
            }
        }

        public static void SetVel(this RootMotionComponent self, Vector2 motionVel)
        {
            self.MotionVel = motionVel;
        }
    }
}