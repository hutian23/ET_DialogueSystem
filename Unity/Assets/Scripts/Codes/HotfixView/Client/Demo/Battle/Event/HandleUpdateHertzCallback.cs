namespace ET.Client
{
    [Invoke]
    public class HandleUpdateHertzCallback : AInvokeHandler<UpdateHertzCallback>
    {
        public override void Handle(UpdateHertzCallback args)
        {
            if (Root.Instance.Get(args.instanceId) is not Unit unit || unit.IsDisposed) return;
            HertzAbility ability = unit.GetComponent<BuffManager>().GetComponent<HertzAbility>();
            ability.SetHertz(args.Hertz);
        }
    }
}