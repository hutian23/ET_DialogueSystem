using Timeline;

namespace ET.Client
{
    [Invoke]
    public class TimelineEditCallback: AInvokeHandler<EditTimelineCallback>
    {
        public override void Handle(EditTimelineCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null)
            {
                return;
            }

            timelineComponent.GetComponent<ScriptParser>().Cancel();
        }
    }
}