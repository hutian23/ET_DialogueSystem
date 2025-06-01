using Box2DSharp.Collision.Shapes;
using Color = Box2DSharp.Common.Color;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [FriendOf(typeof(TargetCheckComponent))]
    public static class TargetCheckComponentSystem
    {
        public class FindTargetComponentDestroySystem : DestroySystem<TargetCheckComponent>
        {
            protected override void Destroy(TargetCheckComponent self)
            {
                self.center = Vector2.Zero;
                self.size = Vector2.Zero;
                self.targetFounded = false;
                self.findTargetCallback_Index = 0;
                self.loseTargetCallback_Index = 0;
            }
        }

        [FriendOf(typeof(BBParser))]
        public class FindTargetComponentPostStepSystem : PostStepSystem<TargetCheckComponent>
        {
            protected override void PosStepUpdate(TargetCheckComponent self)
            {
                // 相关组件
                Unit player = BBUnitHelper.GetPlayer();
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
                b2Body bodyB = b2WorldManager.Instance.GetBody(unit.InstanceId);
                BBParser parser = self.GetParent<BBParser>();

                Vector2 center = bodyB.GetPosition() + self.center * new Vector2(bodyB.GetFlip(), 1);
                self.targetFounded = Box2DHelper.IsPointInBox(bodyA.GetPosition(), center, self.size);

                if (self.targetFounded)
                {
                    // 调用回调
                    if (self.findTargetCallback_Index == 0) return;
                    parser.Invoke(self.findTargetCallback_Index, parser.CancellationToken).Coroutine();
                }
                else
                {
                    if (self.loseTargetCallback_Index == 0) return;
                    parser.Invoke(self.loseTargetCallback_Index, parser.CancellationToken).Coroutine();
                }
            }
        }

        public static bool FindTarget(this TargetCheckComponent self)
        {
            return self.targetFounded;
        }

        public class FindTargetComponentGizmosUpdateSystem : GizmosUpdateSystem<TargetCheckComponent>
        {
            protected override void GizmosUpdate(TargetCheckComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                //1. Position
                Vector2 center = body.GetPosition() + self.center * new Vector2(body.GetFlip(), 1);

                //2. Shape
                PolygonShape shape = new();
                shape.SetAsBox(self.size.X / 2, self.size.Y / 2);

                b2WorldManager.Instance.DrawShape(shape, center, 0, Color.White);
            }
        }
    }
}