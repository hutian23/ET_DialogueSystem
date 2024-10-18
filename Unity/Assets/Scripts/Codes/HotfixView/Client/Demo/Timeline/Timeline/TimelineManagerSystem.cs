﻿namespace ET.Client
{
    [FriendOf(typeof (TimelineManager))]
    [FriendOf(typeof (BBTimerComponent))]
    public static class TimelineManagerSystem
    {
        public class TimelineManagerAwakeSystem: AwakeSystem<TimelineManager>
        {
            protected override void Awake(TimelineManager self)
            {
                TimelineManager.Instance = self;
                self.instanceIds.Clear();
            }
        }

        public static void Update(this TimelineManager self)
        {
            foreach (long instanceId in self.instanceIds)
            {
                EventSystem.Instance.PublishAsync(self.DomainScene(), new UpdateTimelineComponent() { instanceId = instanceId }).Coroutine();
            }
        }

        public class TimelineManagerDestroySystem: DestroySystem<TimelineManager>
        {
            protected override void Destroy(TimelineManager self)
            {
                TimelineManager.Instance = null;
                self.instanceIds.Clear();
            }
        }

        public static void Reload(this TimelineManager self)
        {
            foreach (long instanceId in self.instanceIds)
            {
                TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
                BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
                SkillBuffer skillBuffer = timelineComponent.GetComponent<SkillBuffer>();
                BBParser parser = timelineComponent.GetComponent<BBParser>();
                InputWait inputWait = timelineComponent.GetComponent<InputWait>();
                CancelManager cancelManager = timelineComponent.GetComponent<CancelManager>();

                //1. 重载子组件, 考虑到执行的先后顺序
                bbTimer.ReLoad();
                skillBuffer.Reload();
                inputWait.Reload();
                cancelManager.Cancel();

                //2. 执行各个行为的初始化协程
                parser.Init();
                //3. 进入默认行为
                timelineComponent.Reload(0); // Idle
            }
        }
    }
}