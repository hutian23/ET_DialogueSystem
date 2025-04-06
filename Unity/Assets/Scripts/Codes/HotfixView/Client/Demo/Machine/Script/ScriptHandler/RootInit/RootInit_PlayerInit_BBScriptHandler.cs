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
            
            //1. 热重载时，只保留实现了IController接口的组件
            ListComponent<Entity> removeList = ListComponent<Entity>.Create();
            foreach (Entity child in player.Children.Values)
            {
                if(typeof(IController).IsAssignableFrom(child.GetType())) continue;
                removeList.Add(child);
            }
            foreach (Entity component in player.Components.Values)
            {
                if (typeof(IController).IsAssignableFrom(component.GetType())) continue;
                removeList.Add(component);
            }
            foreach (Entity entity in removeList)
            {
                entity.Dispose();
            }
            removeList.Dispose();
            
            
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