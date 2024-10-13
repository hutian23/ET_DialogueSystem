using Timeline;

namespace ET.Client
{
    public class LoopTimeline_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "LoopTimeline";
        }

        //LoopTimeline
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent timer = timelineComponent.GetComponent<BBTimerComponent>();
            RuntimePlayable playable = timelineComponent.GetTimelinePlayer().RuntimeimePlayable;

            while (!token.IsCancel())
            {
                for (int i = 0; i < playable.ClipMaxFrame(); i++)
                {
                    if (token.IsCancel()) break;
                    timelineComponent.Evaluate(i);
                    await timer.WaitAsync(1, token);
                }
            }

            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}