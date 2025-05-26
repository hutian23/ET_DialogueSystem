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
            HertzAbility ability = unit.GetComponent<BuffManager>().GetComponent<HertzAbility>();
            ability.SetHertz(args.Hertz);
        }
    }
}