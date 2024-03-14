namespace ET.Client
{
    [FriendOf(typeof (BBBehaviorBufferComponent))]
    [FriendOf(typeof (BehaviorInfo))]
    public static class BBBehaviorBufferComponentSystem
    {
        [Invoke(TimerInvokeType.BehaviorBufferTimer)]
        [FriendOf(typeof (BBBehaviorBufferComponent))]
        public class BehaviorBufferTimer: ATimer<BBBehaviorBufferComponent>
        {
            protected override void Run(BBBehaviorBufferComponent self)
            {
                // //1. 根据技能order排序buffer
                // int count = self.BufferQueue.Count;
                // while (count-- > 0)
                // {
                //     BehaviorBuffer buffer = self.BufferQueue.Dequeue();
                //     
                //     bool ret = true;
                //     //2. 检查前置条件
                //     foreach (string trigger in buffer.triggers)
                //     {
                //         Match match = Regex.Match(trigger, @"^\w+");
                //         if (!match.Success)
                //         {
                //             Log.Error($"not found trigger handler: {trigger}");
                //             return;
                //         }
                //
                //         BBParser parser = self.GetParent<BBInputComponent>().GetParent<DialogueComponent>().GetComponent<BBParser>();
                //         BBScriptData data = BBScriptData.Create(trigger, 0);
                //
                //         bool res = DialogueDispatcherComponent.Instance.GetTrigger(match.Value).Check(parser, data);
                //         data.Recycle();
                //
                //         if (res)
                //         {
                //             continue;
                //         }
                //         //不符合条件，取下一个buffer
                //         ret = false;
                //         break;
                //     }
                //     
                //     //3. 符合条件，释放技能
                //     if (ret)
                //     {
                //         
                //         buffer.Recycle();
                //         return;   
                //     }
                //     
                //     self.BufferQueue.Enqueue(buffer);
                // }
             
                
                //1. 遍历行为缓存列表,过期的回收
                int count = self.BufferQueue.Count;
                while (count-- > 0)
                {
                    
                }
                //2. 把当前帧符合条件的行为Order添加到List中，从优先级高的开始遍历，
                
            }
        }

        public class BBBehaviorBufferComponentAwakeSystem: AwakeSystem<BBBehaviorBufferComponent>
        {
            protected override void Awake(BBBehaviorBufferComponent self)
            {
                self.Init();
            }
        }

        public class BBBehaviorBufferComponentDestroySystem : DestroySystem<BBBehaviorBufferComponent>
        {
            protected override void Destroy(BBBehaviorBufferComponent self)
            {
                self.Init();
            }
        }
        
        private static long GetId(this BBBehaviorBufferComponent self)
        {
            return ++self.idGenerator;
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
            TimerComponent.Instance.Remove(ref self.timer);
        }

        public static void AddBehaviorBuffer(this BBBehaviorBufferComponent self, BehaviorInfo skillInfo)
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