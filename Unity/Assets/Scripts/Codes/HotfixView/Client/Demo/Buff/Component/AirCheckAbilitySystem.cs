using System.Collections.Generic;
using System.Numerics;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(AirCheckAbility))]
    public static class AirCheckAbilitySystem
    {
        public class AirCheckComponentAwakeSystem : AwakeSystem<AirCheckAbility>
        {
            protected override void Awake(AirCheckAbility self)
            {
                self.inAir = true;
                self.landVel = Vector2.Zero;
            }
        }

        [FriendOf(typeof(b2Body))]
        public class AirCheckComponentPostStepSystem : PostStepSystem<AirCheckAbility>
        {
            protected override void PosStepUpdate(AirCheckAbility self)
            {
                self.inAir = true;
                
                //1. 查询组件 
                Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                //2. 从碰撞缓冲区中取出碰撞信息，逐个检测
                Queue<CollisionBuffer> bufferQueue = b2Body.collisionStayBuffers;
                int count = bufferQueue.Count;

                while (count-- > 0)
                {
                    CollisionBuffer buffer = bufferQueue.Dequeue();
                    bufferQueue.Enqueue(buffer);

                    //2-1. SquashBox和地面碰撞
                    b2Box boxA = Root.Instance.Get(buffer.instanceIdA) as b2Box;
                    b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;

                    if (boxA.GetBoxType() is not HitboxType.Squash || // 碰撞框 
                        boxB.GetLayerType() is not LayerType.Ground || (boxB.GetTagType() & TagType.Ground) == 0) // 接触地面 
                    {
                        continue;
                    }

                    // //2-2. 这里写的比较简单，只要两个接触点的Y坐标小于中心点即认为落地 
                    // info.Contact.GetWorldManifold(out WorldManifold manifold);
                    // float yMax = Math.Max(manifold.Points[0].Y, manifold.Points[1].Y);
                    //
                    // if (b2Body.GetPosition().Y - yMax < 0f)
                    // {
                    //     continue;
                    // }
                    
                    self.inAir = false;
                    //2-2. 触发落地回调
                    EventSystem.Instance.Invoke(new LandCallback() { instanceId = unit.InstanceId });
                    return;
                }
            }
        }

        public class AirCheckComponentDestroySystem : DestroySystem<AirCheckAbility>
        {
            protected override void Destroy(AirCheckAbility self)
            {
                self.inAir = false;
                self.landVel = Vector2.Zero;
            }
        }

        public static bool GetInAir(this AirCheckAbility self)
        {
            return self.inAir;
        }

        public static void SetLandVel(this AirCheckAbility self, Vector2 vel)
        {
            self.landVel = vel;
        }

        public static Vector2 GetLandVel(this AirCheckAbility self)
        {
            return self.landVel;
        }
    }
}