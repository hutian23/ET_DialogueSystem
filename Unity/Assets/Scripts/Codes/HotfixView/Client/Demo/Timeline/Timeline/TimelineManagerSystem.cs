using Timeline;

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
                BBParser parser = timelineComponent.GetComponent<BBParser>();
                BBTimerComponent timer = timelineComponent.GetComponent<BBTimerComponent>();

                //1. 初始化
                parser.Cancel();
                timer.ReLoad();

                //2. 默认行为
                timelineComponent.GetTimelinePlayer().Init(0); //Idle
                BBTimeline timeline = timelineComponent.GetCurrentTimeline();
                parser.InitScript(timeline.Script);
                parser.Main().Coroutine();
            }
        }
    }
}