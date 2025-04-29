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
                //1. 查询TimelinePlayer
                GameObjectComponent component = self.GetParent<Unit>().GetComponent<GameObjectComponent>();
                TimelinePlayer timelinePlayer = component.GameObject.GetComponent<TimelinePlayer>();
                if (timelinePlayer == null)
                {
                    Log.Error($"gameObject must add TimelinePlayer!!");
                    return;
                }
                
                //2. 渲染层传入unit.instanceId，方便渲染层回调事件
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

        private static void Init(this TimelineComponent self)
        {
            //初始化
            GameObjectComponent component = self.GetParent<Unit>().GetComponent<GameObjectComponent>();
            TimelinePlayer timelinePlayer = component.GameObject.GetComponent<TimelinePlayer>();
            timelinePlayer.RuntimePlayable = null;
            timelinePlayer.CurrentTimeline = null;
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
    }
}