using Box2DSharp.Dynamics;

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
            
            dummy.AddComponent<BuffManager>();
            dummy.AddComponent<Transition>();
            dummy.AddComponent<TimelineComponent>();
            dummy.AddComponent<BBTimerComponent>().IsUnitTimer();
            dummy.AddComponent<BBNumeric>();
            dummy.AddComponent<BehaviorMachine>();
            dummy.AddComponent<B2Unit>();
            dummy.AddComponent<ObjectWait>();
            dummy.GetComponent<GameObjectComponent>().GameObject.transform.SetParent(GlobalComponent.Instance.Unit);

            BuffManager buffManager = dummy.GetComponent<BuffManager>();
            buffManager.AddComponent<HertzAbility>();
            buffManager.AddComponent<HPAbility, int>(100);
            buffManager.AddComponent<SPAbility, int>(100);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}