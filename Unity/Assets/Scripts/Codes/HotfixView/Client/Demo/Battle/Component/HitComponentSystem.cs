using System.Collections.Generic;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(HitComponent))]
    public static class HitComponentSystem
    {
        [Invoke(BBTimerInvokeType.HitNotifyTimer)]
        [FriendOf(typeof(b2Body))]
        [FriendOf(typeof(BBParser))]
        [FriendOf(typeof(HitComponent))]
        public class HitNotifyTimer : BBTimer<BBParser>
        {
            protected override void Run(BBParser self)
            {
                //1. 相关组件
                HitComponent hit = self.GetComponent<HitComponent>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(self.GetParent<Unit>().InstanceId);
                
                //2. 获取缓冲区中的碰撞数据
                Queue<CollisionInfo> infoQueue = b2Body.triggerStayBuffers;
                int count = infoQueue.Count;
                while (count-- > 0)
                {
                    CollisionInfo info = infoQueue.Dequeue();
                    infoQueue.Enqueue(info);

                    //3. 获取碰撞双方的判定框信息
                    BoxInfo infoA = info.dataA.UserData as BoxInfo;
                    BoxInfo infoB = info.dataB.UserData as BoxInfo;
                    if (infoA.hitboxType is not HitboxType.Hit || infoB.hitboxType is not HitboxType.Hurt || info.dataB.InstanceId == 0) continue;

                    //4. 根据instanceId找到对应unit
                    b2Body bodyB = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
                    Unit unitB = Root.Instance.Get(bodyB.unitId) as Unit;

                    //5. 如果受击的Unit已经触发过该攻击回调，是否还要再次调用?
                    if (hit.buffSet.Contains(unitB.InstanceId) && hit.checkType.Equals("Once")) continue;
                    hit.buffSet.Add(unitB.InstanceId);

                    //6. 把碰撞信息注册到共享变量中，供代码块使用
                    hit.curInfo = info;
                    //这里实际上是同步调用的,HitNotify代码块中不能声明 等待相关的指令
                    self.RegistSubCoroutine(hit.startIndex, hit.endIndex, self.CancellationToken).Coroutine();
                    hit.curInfo = default;
                }
            }
        }

        public class HitComponentAwakeSystem : AwakeSystem<HitComponent>
        {
            protected override void Awake(HitComponent self)
            {
                self.Init();
            }
        }
        
        public class HitComponentDestroySystem : DestroySystem<HitComponent>
        {
            protected override void Destroy(HitComponent self)
            {
                self.Init();
            }
        }

        private static void Init(this HitComponent self)
        {
            //移除定时器
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            postStepTimer.Remove(ref self.timer);
            self.startIndex = 0;
            self.endIndex = 0;
            self.checkType = string.Empty;
            self.buffSet.Clear();
            self.curInfo = default;
        }

        public static CollisionInfo GetInfo(this HitComponent self)
        {
            return self.curInfo;
        }
    }
}
