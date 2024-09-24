namespace ET.Client
{
    public class TimelineInit_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "TimelineInit";
        }

        //TimelineInit
        public override async ETTask<Status> Handle(ScriptParser parser, ScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetUnit();
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            timelineComponent.RemoveComponent<BBTimerComponent>();
            timelineComponent.AddComponent<BBTimerComponent>();

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}