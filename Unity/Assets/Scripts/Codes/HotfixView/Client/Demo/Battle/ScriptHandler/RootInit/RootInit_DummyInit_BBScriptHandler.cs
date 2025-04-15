namespace ET.Client
{
    public class RootInit_DummyInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DummyInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit dummy = parser.GetParent<Unit>();
            
            //添加依赖的组件
            dummy.AddComponent<BuffManager>();
            dummy.AddComponent<TimelineComponent>();
            dummy.AddComponent<BBTimerComponent>().IsUnitTimer();
            dummy.AddComponent<BBNumeric>();
            dummy.AddComponent<BehaviorMachine>();
            dummy.AddComponent<B2Unit, long>(dummy.InstanceId);
            dummy.AddComponent<ObjectWait>();
            dummy.GetComponent<GameObjectComponent>().GameObject.transform.SetParent(GlobalComponent.Instance.Unit);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}