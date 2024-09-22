namespace ET.Client
{
    public static class TimelineManagerSystem
    {
        public class TimelineManagerAwakeSystem: AwakeSystem<TimelineManager>
        {
            protected override void Awake(TimelineManager self)
            {
                TimelineManager.Instance = self;
                self.Reload();
            }
        }

        public class TimelineManageLoadSystem: LoadSystem<TimelineManager>
        {
            protected override void Load(TimelineManager self)
            {
                self.Reload();
            }
        }

        private static void Reload(this TimelineManager self)
        {
            EventSystem.Instance.PublishAsync(self.DomainScene(), new InitTimeline()).Coroutine();
        }
    }
}