using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(TimelineComponent))]
    public static class TimelineComponentSystem
    {
        public class TimelineComponentAwakeSystem : AwakeSystem<TimelineComponent>
        {
            protected override void Awake(TimelineComponent self)
            {
                //渲染层传入unit.instanceId，方便渲染层回调事件
                TimelinePlayer timelinePlayer = self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject.GetComponent<TimelinePlayer>();
                if (timelinePlayer == null)
                {
                    Log.Error($"gameObject must add timelineComponent!!");
                    return;
                }
                timelinePlayer.instanceId = self.InstanceId;
            }
        }
        
        public class TimelineComponentDestroySystem : DestroySystem<TimelineComponent>
        {
            protected override void Destroy(TimelineComponent self)
            {
                self.Init();
            }
        }
        
        public class TimelineComponentLoadSystem : LoadSystem<TimelineComponent>
        {
            protected override void Load(TimelineComponent self)
            {
                self.Init();
            }
        }
        
        public static void Init(this TimelineComponent self)
        {
            foreach (var kv in self.markerEventDict)
            {
                TimelineMarkerEvent markerEvent = self.GetChild<TimelineMarkerEvent>(kv.Value);
                markerEvent?.Dispose();
            }
            self.markerEventDict.Clear();
        }
        
        #region TimelinePlayer
        public static TimelinePlayer GetTimelinePlayer(this TimelineComponent self)
        {
            return self.GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<TimelinePlayer>();
        }

        public static void Evaluate(this TimelineComponent self, int targetFrame)
        { 
            self.GetTimelinePlayer().RuntimePlayable.Evaluate(targetFrame);
        }

        #endregion
        
        public static TimelineMarkerEvent GetMarkerEvent(this TimelineComponent self, string eventName)
        {
            if (!self.markerEventDict.TryGetValue(eventName, out long id))
            {
                return null;
            }

            return self.GetChild<TimelineMarkerEvent>(id);
        }
    }
}