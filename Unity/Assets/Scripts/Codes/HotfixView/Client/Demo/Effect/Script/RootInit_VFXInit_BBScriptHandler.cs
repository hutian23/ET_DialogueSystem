namespace ET.Client
{
    public class RootInit_VFXInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VFXInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();

            unit.AddComponent<TimelineComponent>();
            unit.AddComponent<BBTimerComponent>().IsUnitTimer();
            unit.AddComponent<BehaviorMachine>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}