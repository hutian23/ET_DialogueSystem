namespace ET.Client
{
    public class RootInit_EffectInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EffectInit";
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