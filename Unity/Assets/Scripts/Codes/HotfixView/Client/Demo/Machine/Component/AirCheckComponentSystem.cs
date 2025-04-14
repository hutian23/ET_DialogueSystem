using System.Collections.Generic;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(AirCheckComponent))]
    public static class AirCheckComponentSystem
    {
        [Invoke(BBTimerInvokeType.AirCheckTimer)]
        [FriendOf(typeof(B2Unit))]
        [FriendOf(typeof(AirCheckComponent))]
        public class AirCheckTimer : BBTimer<AirCheckComponent>
        {
            protected override void Run(AirCheckComponent self)
            {
                //1. 查询组件 
                Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
                B2Unit b2Unit = unit.GetComponent<B2Unit>();

                //2. 从碰撞缓冲区中取出碰撞信息，逐个检测
                Queue<CollisionInfo> infoQueue = b2Unit.TriggerBuffer;
                int count = infoQueue.Count;
                while (count-- > 0)
                {
                    CollisionInfo info = infoQueue.Dequeue();
                    infoQueue.Enqueue(info);

                    if (info.dataA.Type is not FixtureType.AirCheckBox || // 地面检测盒
                        info.dataB.LayerMask is not LayerType.Ground ||   // 和地面碰撞
                        info.dataB.IsTrigger)                             // 非触发器，实心的
                    {
                        continue;
                    }

                    //触发落地回调
                    if (self.inAir)
                    {
                        EventSystem.Instance.Invoke(new LandCallback() { instanceId = unit.InstanceId });
                    }
                    //落地
                    self.inAir = false;
                    return;
                }

                //3. 在空中
                self.inAir = true;
            }
        }

        public class AirCheckComponentAwakeSystem : AwakeSystem<AirCheckComponent>
        {
            protected override void Awake(AirCheckComponent self)
            {
                BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
                self.timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.AirCheckTimer, self);
                self.inAir = true;
            }
        }

        public class AirCheckComponentDestroySystem : DestroySystem<AirCheckComponent>
        {
            protected override void Destroy(AirCheckComponent self)
            {
                b2WorldManager.Instance.GetPostStepTimer().Remove(ref self.timer);
                self.inAir = false;
            }
        }

        public static bool GetInAir(this AirCheckComponent self)
        {
            return self.inAir;
        }
    }
}