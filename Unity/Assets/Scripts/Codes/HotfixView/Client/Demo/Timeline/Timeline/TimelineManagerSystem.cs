namespace ET.Client
{
    [FriendOf(typeof (TimelineManager))]
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
                timelineComponent.Reload(0); // Idle
            }
        }

        //Edit mode
        public static void Pause(this TimelineManager self,bool pause)
        {
            foreach (var instanceId in self.instanceIds)
            {
                TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
                BBTimerComponent timer = timelineComponent.GetComponent<BBTimerComponent>();

                if (pause)
                {
                    timer.Pause();   
                }
                else
                {
                    timer.Restart();
                }
            }
        }
    }
}