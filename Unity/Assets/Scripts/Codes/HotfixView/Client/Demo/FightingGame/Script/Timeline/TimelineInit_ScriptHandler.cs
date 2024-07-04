namespace ET.Client
{
    public class TimelineInit_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "TimelineInit";
        }

        //TimelineInit
        public override async ETTask<Status> Handle(Unit unit, ScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            timelineComponent.RemoveComponent<BBTimerComponent>();
            timelineComponent.AddComponent<BBTimerComponent>();

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}