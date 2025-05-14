using System;
using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(AirCheckAbility))]
    public static class AirCheckAbilitySystem
    {
        [Invoke(BBTimerInvokeType.AirCheckTimer)]
        [FriendOf(typeof(AirCheckAbility))]
        [FriendOf(typeof(b2Body))]
        public class AirCheckTimer : BBTimer<AirCheckAbility>
        {
            protected override void Run(AirCheckAbility self)
            {
                //1. 查询组件 
                Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                //2. 从碰撞缓冲区中取出碰撞信息，逐个检测
                Queue<CollisionInfo> infoQueue = b2Body.collisionStayBuffer;
                int count = infoQueue.Count;

                while (count-- > 0)
                {
                    CollisionInfo info = infoQueue.Dequeue();
                    infoQueue.Enqueue(info);

                    //2-1. PushBox和地面碰撞
                    BoxInfo infoA = info.dataA.UserData as BoxInfo;
                    if (infoA.hitboxType is not HitboxType.Squash || info.dataB.LayerMask is not LayerType.Ground)
                    {
                        continue;
                    }
                    
                    //2-2. 这里写的比较简单，只要两个接触点的Y坐标小于中心点即认为落地 
                    info.Contact.GetWorldManifold(out WorldManifold manifold);
                    float yMax = Math.Max(manifold.Points[0].Y, manifold.Points[1].Y);
                    
                    if (b2Body.GetPosition().Y - yMax < 0f)
                    {
                        continue;
                    }

                    //2-3. 落地回调
                    if (!self.inAir)
                    {
                        return;
                    }
                    self.inAir = false;
                    EventSystem.Instance.Invoke(new LandCallback() { instanceId = unit.InstanceId });
                    return;
                }

                //3. 在空中
                self.inAir = true;
            }
        }

        public class AirCheckComponentAwakeSystem : AwakeSystem<AirCheckAbility>
        {
            protected override void Awake(AirCheckAbility self)
            {
                BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
                self.timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.AirCheckTimer, self);
                self.inAir = true;
            }
        }

        public class AirCheckComponentDestroySystem : DestroySystem<AirCheckAbility>
        {
            protected override void Destroy(AirCheckAbility self)
            {
                b2WorldManager.Instance.GetPostStepTimer().Remove(ref self.timer);
                self.inAir = false;
                self.landV_X = 0f;
                self.landV_Y = 0f;
            }
        }

        public static bool GetInAir(this AirCheckAbility self)
        {
            return self.inAir;
        }

        public static void SetLandVel(this AirCheckAbility self, Vector2 vel)
        {
            self.landV_X = vel.X;
            self.landV_Y = vel.Y;
        }

        public static Vector2 GetLandVel(this AirCheckAbility self)
        {
            return new Vector2(self.landV_X, self.landV_Y);
        }
    }
}