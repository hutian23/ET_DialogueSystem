namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBParser))]
    public class HandleLandCallback : AInvokeHandler<LandCallback>
    {
        public override void Handle(LandCallback args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            if (unit == null || unit.InstanceId == 0)
            {
                Log.Error($"cannot found behavior machine: {args.instanceId}");
                return;
            }
            
            BBParser parser = unit.GetComponent<BBParser>();

            if (!parser.ContainFunction("Root", "LandCallback")) return;
            parser.Invoke(parser.GetFunctionPointer("Root", "LandCallback"), parser.CancellationToken).Coroutine();

        }
    }
}
