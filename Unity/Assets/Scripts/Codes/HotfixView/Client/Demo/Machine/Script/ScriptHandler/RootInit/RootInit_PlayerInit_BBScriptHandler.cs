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

            //1. 初始化组件
            player.RemoveComponent<TimelineComponent>();
            player.RemoveComponent<BBTimerComponent>();
            player.RemoveComponent<BBNumeric>();
            player.RemoveComponent<BehaviorMachine>();
            player.RemoveComponent<B2Unit>();
            player.RemoveComponent<ObjectWait>();
            player.RemoveComponent<InputWait>();
            
            //2. 添加需要的组件
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