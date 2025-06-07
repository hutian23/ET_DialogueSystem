using System.Linq;
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
                    Log.Error($"GameObject must add TimelinePlayer component!!!");
                    return;
                }
                timelinePlayer.Dispose();
                
                //2. 渲染层传入unit.instanceId，方便渲染层回调事件
                timelinePlayer.instanceId = self.InstanceId;
            }
        }
        
        public class TimelineComponentDestroySystem : DestroySystem<TimelineComponent>
        {
            protected override void Destroy(TimelineComponent self)
            {
                Dispose(self);
            }
        }
        
        public class TimelineComponentLoadSystem : LoadSystem<TimelineComponent>
        {
            protected override void Load(TimelineComponent self)
            {
                Dispose(self);
            }
        }

        private static void Dispose(this TimelineComponent self)
        {
            // Unit被销毁，GameObjectComponent的添加顺序在timeline前，避免空引用
            if (self.GetParent<Unit>().InstanceId == 0)
            {
                return;
            }
            GameObjectComponent component = self.GetParent<Unit>().GetComponent<GameObjectComponent>();
            TimelinePlayer timelinePlayer = component.GameObject.GetComponent<TimelinePlayer>();
            timelinePlayer.Dispose();
        }

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

        public static int GetTargetFrame(this TimelineComponent self, string markerName)
        {
            RuntimePlayable runtimePlayable = self.GetTimelinePlayer().RuntimePlayable;
            foreach (RuntimeTrack runtimeTrack in runtimePlayable.runtimeTracks)
            {
                if (runtimeTrack.Track is not BBEventTrack eventTrack) continue;
                if (eventTrack.Name.Equals("Marker"))
                {
                    EventInfo info = eventTrack.EventInfos.FirstOrDefault(info => info.keyframeName.Equals(markerName));
                    if (info == null)
                    {
                        Log.Error($"not found marker:{markerName}");
                        return -1;
                    }
                    
                    return info.frame;
                }
            }

            return -1;
        }
    }
}