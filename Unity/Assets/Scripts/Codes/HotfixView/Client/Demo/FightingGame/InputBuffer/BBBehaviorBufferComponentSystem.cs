using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBBehaviorBufferComponent))]
    [FriendOf(typeof (BBSkillInfo))]
    public static class BBBehaviorBufferComponentSystem
    {
        [Invoke(TimerInvokeType.BehaviorBufferTimer)]
        [FriendOf(typeof (BBBehaviorBufferComponent))]
        public class BehaviorBufferTimer: ATimer<BBBehaviorBufferComponent>
        {
            protected override void Run(BBBehaviorBufferComponent self)
            {
                //1.移除过期buffer
                int count = self.BufferQueue.Count;
                while (count-- > 0)
                {
                    BehaviorBuffer buffer = self.BufferQueue.Dequeue();
                    //过期，回收
                    if (self.GetNow() > buffer.startFrame + buffer.LastedFrame)
                    {
                        buffer.Recycle();
                        continue;
                    }

                    self.BufferQueue.Enqueue(buffer);
                }

                //2. 根据技能order排序buffer
                count = self.BufferQueue.Count;
                self.workDict.Clear();
                while (count-- > 0)
                {
                    BehaviorBuffer buffer = self.BufferQueue.Dequeue();
                    self.workDict.TryAdd(buffer.skillOrder, buffer);
                    self.BufferQueue.Enqueue(buffer);
                }

                //3. 判断技能前置条件
                for (int i = 0; i < self.workDict.Count; i++)
                {
                    BehaviorBuffer buffer = self.workDict[i];
                    bool ret = true;
                    foreach (string trigger in buffer.triggers)
                    {
                        Match match = Regex.Match(trigger, @":\s*(\w+)");
                        if (!match.Success)
                        {
                            Log.Error($"not found trigger handler: {trigger}");
                            return;
                        }

                        BBParser parser = self.GetParent<BBInputComponent>().GetParent<DialogueComponent>().GetComponent<BBParser>();
                        BBScriptData data = BBScriptData.Create(trigger, 0);

                        bool res = DialogueDispatcherComponent.Instance.GetTrigger(match.Groups[1].Value).Check(parser, data);
                        data.Recycle();

                        if (res) continue;
                        ret = false;
                        break;
                    }

                    //符合条件，释放技能
                    if (!ret) continue;
                    ObjectWait wait = self.GetParent<BBInputComponent>().GetParent<DialogueComponent>().GetComponent<ObjectWait>();
                    wait.Notify(new WaitNextSkill() { targetID = buffer.targetID });
                    buffer.Recycle();
                    return;
                }
            }
        }

        public class BBBehaviorBufferComponentAwakeSystem: AwakeSystem<BBBehaviorBufferComponent>
        {
            protected override void Awake(BBBehaviorBufferComponent self)
            {
                self.Init();
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

        private static void Init(this BBBehaviorBufferComponent self)
        {
            int count = self.BufferQueue.Count;
            while (count-- > 0)
            {
                BehaviorBuffer buffer = self.BufferQueue.Dequeue();
                buffer.Recycle();
            }

            self.BufferQueue.Clear();
            self.workDict.Clear();
            TimerComponent.Instance.Remove(ref self.timer);
        }

        public static void AddBehaviorBuffer(this BBBehaviorBufferComponent self, BBSkillInfo skillInfo)
        {
            if (self.BufferQueue.Count >= 100)
            {
                self.BufferQueue.Dequeue();
            }

            BBTimerComponent timerComponent = self.GetParent<BBInputComponent>().GetComponent<BBTimerComponent>();
            BehaviorBuffer buffer = BehaviorBuffer.Create(self.GetId(),
                skillInfo.targetID,
                skillInfo.GetSkillOrder(),
                timerComponent.GetNow(),
                skillInfo.LastedFrame,
                skillInfo.triggers);
            self.BufferQueue.Enqueue(buffer);
        }

        public static void EnableBehaviorBufferCheckTimer(this BBBehaviorBufferComponent self)
        {
            self.timer = TimerComponent.Instance.NewFrameTimer(TimerInvokeType.BehaviorBufferTimer, self);
        }

        public static void DisableBehaviorBufferCheckTimer(this BBBehaviorBufferComponent self)
        {
            TimerComponent.Instance.Remove(ref self.timer);
        }
    }
}