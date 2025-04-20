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

            //2. 进行充能
            if (buffManager.GetComponent<GroundDashRecharge>() == null)
            {
                buffManager.AddComponent<GroundDashRecharge, int>(gd.GetChargeFrame(), true);
            }
        }
    }
}