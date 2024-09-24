using Timeline;

namespace ET.Client
{
    [FriendOf(typeof (ScriptDispatcherComponent))]
    public class StartTimeline_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "StartTimeline";
        }

        public override async ETTask<Status> Handle(ScriptParser parser, ScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetUnit();

            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BBTimerComponent timerComponent = timelineComponent.GetComponent<BBTimerComponent>();
            RuntimePlayable runtimePlayable = timelineComponent.GetTimelinePlayer().RuntimeimePlayable;

            for (int i = 0; i < runtimePlayable.ClipMaxFrame(); i++)
            {
                if (token.IsCancel())
                {
                    return Status.Failed;
                }

                runtimePlayable.Evaluate(i);
                await timerComponent.WaitFrameAsync(token);
            }

            return Status.Success;
        }
    }
}