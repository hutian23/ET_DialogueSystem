namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorMachine))]    
    // 行为机模块对所有关联的模块进行初始化
    public class HandleReloadCallback : AInvokeHandler<ReloadCallback>
    {
        public override void Handle(ReloadCallback args)
        {
            BehaviorMachine machine = Root.Instance.Get(args.instanceId) as BehaviorMachine;
            if (machine == null || machine.InstanceId == 0)
            {
                Log.Error($"cannot found behaviorMachine");
                return;
            }
            
            Unit unit = machine.GetParent<Unit>();
            BBParser parser = unit.GetComponent<BBParser>();
            
            if (!parser.ContainFunction("Root", "ReloadCallback"))
            {
                return;
            }
            parser.Invoke(parser.GetFunctionPointer("Root", "ReloadCallback"), parser.CancellationToken).Coroutine();
        }
    }
}