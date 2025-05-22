using System.Collections.Generic;
using ET.Event;
using Timeline;

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
                self.info = default;
            }
        }

        [FriendOf(typeof(b2Body))]
        [FriendOf(typeof(BBParser))]
        public class HurtComponentPostStepSystem : PostStepSystem<HurtComponent>
        {
            protected override void PosStepUpdate(HurtComponent self)
            {
                //1. 相关组件
                BBParser parser = self.GetParent<BBParser>();
                Unit unitA = parser.GetParent<Unit>();
                b2Body bodyA = b2WorldManager.Instance.GetBody(unitA.InstanceId);

                //2. 获取缓冲区中的碰撞数据
                Queue<CollisionInfo> infoQueue = bodyA.triggerStayBuffers;
                int count = infoQueue.Count;
                while (count-- > 0)
                {
                    CollisionInfo info = infoQueue.Dequeue();
                    infoQueue.Enqueue(info);

                    //3. 获取碰撞双方的判定框信息
                    BoxInfo infoA = info.dataA.UserData as BoxInfo;
                    BoxInfo infoB = info.dataB.UserData as BoxInfo;
                    if (infoA.hitboxType is not HitboxType.Hurt || infoB.hitboxType is not HitboxType.Hit || info.dataB.InstanceId == 0) continue;

                    //4. 根据instanceId找到对应Unit
                    b2Body bodyB = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
                    Unit unitB = Root.Instance.Get(bodyB.unitId) as Unit;

                    //5. 如果受击的unit触发过该受击回调，是否还要再次调用
                    if (self.buffSet.Contains(unitB.InstanceId) && self.checkType.Equals("Once")) continue;
                    self.buffSet.Add(unitB.InstanceId);

                    //6. 执行代码块
                    self.info = info;
                    parser.RegistSubCoroutine(self.startIndex, self.endIndex, parser.CancellationToken).Coroutine();
                    self.info = default;
                }
            }
        }
    }
}