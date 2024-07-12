using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleTimelineEventCallback: AInvokeHandler<EventMarkerCallback>
    {
        public override void Handle(EventMarkerCallback args)
        {
            Log.Warning($"Handle TimelineEvent:{args.info.keyframeName}");
        }
    }
}