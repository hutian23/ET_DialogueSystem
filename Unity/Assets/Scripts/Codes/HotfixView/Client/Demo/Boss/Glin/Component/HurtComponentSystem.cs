using System.Collections.Generic;
using ET.Event;

namespace ET.Client
{
    public static class HurtComponentSystem
    {
        public class HurtComponentDestroySystem : DestroySystem<HurtComponent>
        {
            protected override void Destroy(HurtComponent self)
            {
                self.startIndex = 0;
                self.endIndex = 0;
                self.checkType = string.Empty;
                self.buffSet.Clear();
                self.buffer = default;
            }
        }

        [FriendOf(typeof(b2Body))]
        [FriendOf(typeof(BBParser))]
        public class HurtComponentPostStepSystem : PostStepSystem<HurtComponent>
        {
            protected override void PosStepUpdate(HurtComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                Queue<CollisionBuffer> bufferQueue = body.triggerStayBuffers;
                int count = bufferQueue.Count;
                while (count -- > 0)
                {
                    CollisionBuffer buffer = bufferQueue.Dequeue();
                    
                    //1. 获取碰撞双方的判定框信息
                    b2Box boxA = Root.Instance.Get(buffer.instanceIdA) as b2Box;
                    b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
                    if(boxA == null || boxB == null || boxA.IsDisposed || boxB.IsDisposed || 
                       boxA.GetBoxType() is not HitboxType.Hurt || boxB.GetBoxType() is not HitboxType.Hit) continue;
                    
                    //2. 根据instanceId找到对应unit
                    b2Body bodyA = boxA.GetParent<b2Body>();
                    b2Body bodyB = boxB.GetParent<b2Body>();
                    Unit unitA = Root.Instance.Get(bodyA.unitId) as Unit;
                    Unit unitB = Root.Instance.Get(bodyB.unitId) as Unit;
                    
                    //3. 如果受击的unit触发过该受击回调，是否还要再次调用
                    if (self.buffSet.Contains(unitB.InstanceId) && self.checkType.Equals("Once")) continue;
                    self.buffSet.Add(unitB.InstanceId);
                    
                    //4. 执行受击回调
                    BBParser parser = unitA.GetComponent<BBParser>();
                    self.buffer = buffer;
                    parser.RegistSubCoroutine(self.startIndex, self.endIndex, parser.CancellationToken).Coroutine();
                    self.buffer = default;
                }
            }
        }
    }
}