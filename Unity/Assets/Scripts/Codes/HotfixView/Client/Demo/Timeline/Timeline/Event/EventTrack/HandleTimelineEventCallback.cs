using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleTimelineEventCallback: AInvokeHandler<EventMarkerCallback>
    {
        public override void Handle(EventMarkerCallback args)
        {
            // TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            // if (timelineComponent == null) return;

            // TimelineEventManager manager = timelineComponent.GetComponent<TimelineEventManager>();

            //动画帧事件
            // ScriptParser parser = manager.GetParser(args.track.Name);
            // parser.Invoke("Main").Coroutine();
        }
    }
}