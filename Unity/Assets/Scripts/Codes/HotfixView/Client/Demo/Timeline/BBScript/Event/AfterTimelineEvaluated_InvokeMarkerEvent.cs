using Timeline;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterTimelineEvaluated_InvokeMarkerEvent: AEvent<AfterTimelineEvaluated>
    {
        protected override async ETTask Run(Scene scene, AfterTimelineEvaluated args)
        {
            //1. Find MarkerTrack
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            RuntimePlayable runtimePlayable = timelineComponent.GetTimelinePlayer().RuntimeimePlayable;

            BBEventTrack markerTrack = null;
            foreach (RuntimeTrack runtimeTrack in runtimePlayable.RuntimeTracks)
            {
                if (runtimeTrack.Track is not BBEventTrack eventTrack)
                {
                    continue;
                }

                if (eventTrack.Name != "Marker")
                {
                    continue;
                }

                markerTrack = eventTrack;
            }

            if (markerTrack == null)
            {
                return;
            }

            //2. 有无注册动画帧事件，有则执行协程
            EventInfo eventInfo = markerTrack.GetInfo(args.targetFrame);
            if (eventInfo == null)
            {
                return;
            }

            MarkerEventParser eventParser = timelineComponent.GetComponent<BBParser>().GetComponent<MarkerEventParser>();
            if (eventParser is not null && eventParser.ContainMarker(eventInfo.keyframeName))
            {
                eventParser.Invoke(eventInfo.keyframeName);
            }

            await ETTask.CompletedTask;
        }
    }
}