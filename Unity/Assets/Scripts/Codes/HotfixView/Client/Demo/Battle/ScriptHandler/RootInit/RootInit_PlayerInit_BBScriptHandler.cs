namespace ET.Client
{
    public class RootInit_PlayerInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "PlayerInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit player = parser.GetParent<Unit>();
            
            //添加玩家控制器依赖的组件
            player.AddComponent<BuffManager>();
            player.AddComponent<TimelineComponent>();
            player.AddComponent<BBTimerComponent>().IsUnitTimer();
            player.AddComponent<BBNumeric>();
            player.AddComponent<BehaviorMachine>();
            player.AddComponent<B2Unit, long>(player.InstanceId);
            player.AddComponent<ObjectWait>();
            player.AddComponent<InputWait>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}