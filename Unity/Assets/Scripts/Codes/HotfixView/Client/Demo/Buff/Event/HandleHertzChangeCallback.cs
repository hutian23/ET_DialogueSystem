namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBTimerComponent))]
    public class HandleHertzChangeCallback : AInvokeHandler<HertzChangeCallback>
    {
        public override void Handle(HertzChangeCallback args)
        {
            //1. 查询组件
            HertzAbility hertzAbility = Root.Instance.Get(args.instanceId) as HertzAbility;
            Unit unit = hertzAbility.GetParent<BuffManager>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();

            //2. 更新战斗定时器Hertz
            bbTimer.SetHertz(args.hertz);
            bbTimer.Accumulator = 0;

            //3. 移速随Hertz缩放
            b2Unit.SetHertz(args.hertz);
        }
    }
}