using Timeline;
using UnityEngine;

namespace ET.Client
{
    public static class TimelineComponentSystem
    {
        public class BBTimelineComponentAwakeSystem: AwakeSystem<TimelineComponent>
        {
            protected override void Awake(TimelineComponent self)
            {
                GameObject go = self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
                TimelinePlayer timelinePlayer = go.GetComponent<TimelinePlayer>();
                timelinePlayer.instanceId = self.InstanceId;
            }
        }

        public static T GetParameter<T>(this TimelineComponent timelineComponent, string parameterName)
        {
            TimelinePlayer timelinePlayer = timelineComponent.GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<TimelinePlayer>();
            BBPlayableGraph playableGraph = timelinePlayer.BBPlayable;
            foreach (var param in playableGraph.Parameters)
            {
                if (param.name == parameterName)
                {
                    if (param.value is not T value)
                    {
                        Log.Error($"cannot format {param.name} to {typeof (T)}");
                        return default;
                    }

                    return value;
                }
            }

            return default;
        }

        public static object GetParameter(this TimelineComponent self, string paramName)
        {
            TimelinePlayer timelinePlayer = self.GetTimelinePlayer();

            BBPlayableGraph playableGraph = timelinePlayer.BBPlayable;
            foreach (SharedVariable param in playableGraph.Parameters)
            {
                if (param.name == paramName)
                {
                    return param.value;
                }
            }

            return null;
        }

        public static TimelinePlayer GetTimelinePlayer(this TimelineComponent self)
        {
            return self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<TimelinePlayer>();
        }
    }
}