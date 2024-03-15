using Sirenix.Utilities;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorBufferComponent))]
    public static class BehaviorBufferComponentSystem
    {
        [Invoke(BBTimerInvokeType.BehaviorBufferCheckTimer)]
        [FriendOf(typeof (BehaviorBufferComponent))]
        public class BehaviorBufferCheckTimer: BBTimer<BehaviorBufferComponent>
        {
            protected override void Run(BehaviorBufferComponent self)
            {
                BBTimerComponent bbtimer = self.GetParent<DialogueComponent>().GetComponent<BBTimerComponent>();

                int count = self.BufferQueue.Count;
                self.OrderSet.Clear();
                while (count-- > 0)
                {
                    BehaviorBuffer buffer = self.BufferQueue.Dequeue();
                    //检查是否过期，过期则回收
                    if (bbtimer.GetNow() > buffer.startFrame + buffer.LastedFrame)
                    {
                        buffer.Recycle();
                        continue;
                    }

                    //缓存当前帧所有可执行的行为
                    self.OrderSet.Add(buffer.order);
                    self.BufferQueue.Enqueue(buffer);
                }
            }
        }

        public static bool ContainOrder(this BehaviorBufferComponent self, uint skillType, uint order)
        {
            ulong result = 0;
            result |= order;
            result |= (ulong)skillType << 16;
            return self.ContainOrder((long)result);
        }

        public static bool ContainOrder(this BehaviorBufferComponent self, long skillOrder)
        {
            return self.OrderSet.Contains(skillOrder);
        }

        private static void Init(this BehaviorBufferComponent self)
        {
            self.OrderSet.Clear();
            self.BufferQueue.ForEach(buffer => { buffer.Recycle(); });
            self.GetParent<DialogueComponent>()
                    .GetComponent<BBInputComponent>()?
                    .GetComponent<BBTimerComponent>()?
                    .Remove(ref self.bufferCheckTimer);
        }

        public static void EnableBehaviorBufferCheck(this BehaviorBufferComponent self)
        {
            self.GetParent<DialogueComponent>()
                    .GetComponent<BBInputComponent>()
                    .GetComponent<BBTimerComponent>()
                    .NewFrameTimer(BBTimerInvokeType.BehaviorBufferCheckTimer, self);
        }

        public class BehaviorBufferComponentDestroySystem: DestroySystem<BehaviorBufferComponent>
        {
            protected override void Destroy(BehaviorBufferComponent self)
            {
                self.Init();
            }
        }
    }
}