namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBTimerComponent))]
    public class HandleHertzChangeCallback : AInvokeHandler<HertzChangeCallback>
    {
        public override void Handle(HertzChangeCallback args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;

            // 逻辑帧
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            bbTimer.SetHertz(args.hertz);
            bbTimer.Accumulator = 0;

            // 物理帧
            if (b2WorldManager.Instance.ContainBody(unit.InstanceId))
            {
                b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
                body.SetHertz(args.hertz);
            }
        }
    }
}