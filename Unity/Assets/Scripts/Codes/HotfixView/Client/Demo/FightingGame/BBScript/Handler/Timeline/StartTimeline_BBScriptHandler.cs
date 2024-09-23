using Timeline;

namespace ET.Client
{
    public class StartTime_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "StartTimeline";
        }

        //StartTimeline;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // DialogueComponent dialogueComponent = parser.GetParent<DialogueComponent>();
            // BBTimerComponent bbTimer = dialogueComponent.GetComponent<BBTimerComponent>();
            // RootMotionComponent rootMotion = dialogueComponent.GetComponent<RootMotionComponent>();
            // TimelineManager timelineManager = dialogueComponent.GetComponent<TimelineManager>();
            //
            // rootMotion.Init(data.targetID);
            //
            // for (int i = 0; i <= timelineManager.GetPlayable().ClipMaxFrame(); i++)
            // {
            //     timelineManager.GetPlayable().Evaluate(i);
            //     
            //     //Update root motion
            //     rootMotion.UpdatePos(i);
            //     
            //     await bbTimer.WaitAsync(1, token);
            //     if (token.IsCancel())
            //     {
            //         return Status.Failed;
            //     }
            // }
            //
            // rootMotion.OnDone();
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent timer = timelineComponent.GetComponent<BBTimerComponent>();
            RuntimePlayable playable = timelineComponent.GetTimelinePlayer().RuntimeimePlayable;

            for (int i = 0; i < playable.ClipMaxFrame(); i++)
            {
                playable.Evaluate(i);
                await timer.WaitAsync(1, token);
                if (token.IsCancel()) break;
            }

            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}