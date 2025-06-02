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
            player.AddComponent<Transition>();
            player.AddComponent<TimelineComponent>();
            player.AddComponent<BBTimerComponent>().IsUnitTimer();
            player.AddComponent<BBNumeric>();
            player.AddComponent<BehaviorMachine>();
            player.AddComponent<ObjectWait>();
            player.AddComponent<InputWait>();
            player.AddComponent<B2Unit>();
            
            //添加能力
            BuffManager buffManager = player.GetComponent<BuffManager>();
            buffManager.AddComponent<HPAbility, int>(100);
            buffManager.AddComponent<SPAbility, int>(100);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}