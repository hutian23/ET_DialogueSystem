namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2WorldManager))]
    [FriendOf(typeof(b2Body))]
    public class HandleDisposeB2bodyCallback : AInvokeHandler<DisposeB2bodyCallback>
    {
        public override void Handle(DisposeB2bodyCallback args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            if (unit == null || unit.InstanceId == 0)
            {
                return;
            }
            
            b2WorldManager.Instance.DestroyBody(unit.InstanceId);
        }
    }
}