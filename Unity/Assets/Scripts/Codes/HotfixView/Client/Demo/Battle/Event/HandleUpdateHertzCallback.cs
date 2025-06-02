namespace ET.Client
{
    [Invoke]
    public class HandleUpdateHertzCallback : AInvokeHandler<UpdateHertzCallback>
    {
        public override void Handle(UpdateHertzCallback args)
        {
            EventSystem.Instance.Invoke(new HertzChangeCallback(){instanceId = args.instanceId, hertz = args.Hertz});
        }
    }
}