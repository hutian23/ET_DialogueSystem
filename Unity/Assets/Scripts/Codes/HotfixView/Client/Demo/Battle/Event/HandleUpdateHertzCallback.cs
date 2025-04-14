using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleUpdateHertzCallback : AInvokeHandler<UpdateHertzCallback>
    {
        public override void Handle(UpdateHertzCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null || timelineComponent.InstanceId == 0)
            {
                Log.Error($"cannot found TimelineComponent: {args.instanceId}");
                return;
            }

            Unit unit = timelineComponent.GetParent<Unit>();
            BBNumeric numeric = unit.GetComponent<BBNumeric>();
            numeric.Set("Hertz", args.Hertz);
        }
    }
}