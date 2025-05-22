using System.Collections.Generic;
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
        
        public class AirDashToGroundComponent_AwakeSystem : AwakeSystem<AirDashToGroundComponent>
        {
            protected override void Awake(AirDashToGroundComponent self)
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
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                Queue<CollisionInfo> infoQueue = b2Body.collisionStayBuffers;
                int count = infoQueue.Count;

                while (count-- > 0)
                {
                    CollisionInfo info = infoQueue.Dequeue();
                    infoQueue.Enqueue(info);
                    
                    //1. 获取碰撞双方判定框信息
                    BoxInfo infoA = info.dataA.UserData as BoxInfo;
                    BoxInfo infoB = info.dataB.UserData as BoxInfo;
                    if (infoA.hitboxType is not HitboxType.Squash ||
                        info.dataB.LayerType is not LayerType.Ground ||
                        infoB.tagType is not TagType.Ground) continue;

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