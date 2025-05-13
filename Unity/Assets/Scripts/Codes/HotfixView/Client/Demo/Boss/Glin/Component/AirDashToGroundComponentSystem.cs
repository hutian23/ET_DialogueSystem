using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(AirDashToGroundComponent))]
    public static class AirDashToGroundComponentSystem
    {
        public class AirDashToGroundComponent_DestroySystem : DestroySystem<AirDashToGroundComponent>
        {
            protected override void Destroy(AirDashToGroundComponent self)
            {
                self.OnGround = false;
            }
        }

        [FriendOf(typeof(b2Body))]
        [FriendOf(typeof(AirCheckAbility))]
        public class AirDashToGroundComponent_PostStepSystem : PostStepSystem<AirDashToGroundComponent>
        {
            protected override void PosStepUpdate(AirDashToGroundComponent self)
            {
                //1. 查询组件
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                //2. 从碰撞缓冲区中取出碰撞信息，逐个检测
                Queue<CollisionInfo> infoQueue = b2Body.CollisionStayBuffer;
                int count = infoQueue.Count;

                while (count-- > 0)
                {
                    CollisionInfo info = infoQueue.Dequeue();
                    infoQueue.Enqueue(info);

                    //2-1 PushBox和地面碰撞
                    BoxInfo infoA = info.dataA.UserData as BoxInfo;
                    if (infoA.hitboxType is not HitboxType.Squash || info.dataB.LayerMask is not LayerType.Ground) continue;
                    
                    //2-2 法向量垂直于地面
                    info.Contact.GetWorldManifold(out WorldManifold manifold);
                    if (manifold.Normal != new Vector2(0, 1)) continue;

                    self.OnGround = true;
                    return;
                }

                self.OnGround = false;
            }
        }

        public static bool GetOnGround(this AirDashToGroundComponent self)
        {
            return self.OnGround;
        }
    }
}