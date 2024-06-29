using Timeline;
using UnityEngine;

namespace ET.Client
{
    public static class BBTimelineComponentSystem
    {
        public class BBTimelineComponentAwakeSystem: AwakeSystem<BBTimelineComponent>
        {
            protected override void Awake(BBTimelineComponent self)
            {
                GameObject go = self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
                TimelinePlayer timelinePlayer = go.GetComponent<TimelinePlayer>();
                timelinePlayer.instanceId = self.InstanceId;
            }
        }
    }
}