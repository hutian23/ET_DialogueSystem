namespace ET.Client
{
    [Invoke]
    public class HandleGroundDashChangeCallback : AInvokeHandler<GroundDashChangeCallback>
    {
        public override void Handle(GroundDashChangeCallback args)
        {
            //1. 查询组件
            GroundDashAbility gd = Root.Instance.Get(args.instanceId) as GroundDashAbility;
            BuffManager buffManager = gd.GetParent<BuffManager>();
            
            //2. 当前冲刺次数充满，不需要充能
            if (gd.GetDashCount() == gd.GetMaxDashCount())
            {
                buffManager.RemoveComponent<GroundDashRecharge>();
            }
            else if (buffManager.GetComponent<GroundDashRecharge>() == null)
            {
                buffManager.AddComponent<GroundDashRecharge, int>(gd.GetChargeFrame(), true);
            }
        }
    }
}