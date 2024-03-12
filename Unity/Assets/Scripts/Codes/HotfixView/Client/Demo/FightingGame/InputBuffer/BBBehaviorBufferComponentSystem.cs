using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (BBBehaviorBufferComponent))]
    public static class BBBehaviorBufferComponentSystem
    {
        public class BBBehaviorBufferComponentAwakeSystem: AwakeSystem<BBBehaviorBufferComponent>
        {
            protected override void Awake(BBBehaviorBufferComponent self)
            {
                self.Init();
            }
        }

        public class BBBehaviorBufferComponentUpdateSystem: UpdateSystem<BBBehaviorBufferComponent>
        {
            protected override void Update(BBBehaviorBufferComponent self)
            {
            }
        }

        private static long GetId(this BBBehaviorBufferComponent self)
        {
            return ++self.idGenerator;
        }

        /// <summary>
        /// 当前帧号
        /// </summary>
        private static long GetNow(this BBBehaviorBufferComponent self)
        {
            BBTimerComponent timerComponent = self.GetParent<BBInputComponent>().GetComponent<BBTimerComponent>();
            return timerComponent.GetNow();
        }

        /// <summary>
        /// 当前帧 - 保质帧 ,最小有效帧
        /// </summary>
        public static long GetMinFrame(this BBBehaviorBufferComponent self)
        {
            return self.GetNow() - 5;
        }

        public static void BufferUpdate(this BBBehaviorBufferComponent self)
        {
            if (self.bufferIds.Count == 0)
            {
                return;
            }

            //1. 移除过期的缓存
            foreach (KeyValuePair<long, List<long>> kv in self.bufferIds)
            {
                long k = kv.Key;
                if (k < self.GetMinFrame()) self.timeOutQueue.Enqueue(k);
            }

            while (self.timeOutQueue.Count > 0)
            {
                long timeId = self.timeOutQueue.Dequeue();
                List<long> bufferIds = self.bufferIds[timeId];
                for (int i = 0; i < bufferIds.Count; i++)
                {
                    long bufferId = bufferIds[i];
                    BehaviorBuffer buffer = self.bufferDict[bufferId];
                    buffer.Recycle();
                }

                self.bufferIds.Remove(timeId);
            }
            //2. 
        }

        private static void Init(this BBBehaviorBufferComponent self)
        {
            self.bufferDict.Clear();
            self.bufferIds.Clear();
        }

        public static void AddBehaviorBuffer(this BBBehaviorBufferComponent self, string skillTag, long skillOrder, long lastedFrame, List<string> triggers)
        {
            BBTimerComponent timerComponent = self.GetParent<BBInputComponent>().GetComponent<BBTimerComponent>();
            BehaviorBuffer buffer = BehaviorBuffer.Create(self.GetId(), skillTag, skillOrder, timerComponent.GetNow(), lastedFrame, triggers);

            self.bufferDict.Add(buffer.Id, buffer);
            self.bufferIds.Add(buffer.startFrame, self.GetId());
        }
    }
}