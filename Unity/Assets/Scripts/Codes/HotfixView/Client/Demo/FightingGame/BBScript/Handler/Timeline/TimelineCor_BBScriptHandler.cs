using Timeline;

namespace ET.Client
{
    public class TimelineCor_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "StartTimelineCor";
        }

        //StartTimelineCor;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = parser.GetParent<DialogueComponent>();
            BBTimerComponent bbTimer = dialogueComponent.GetComponent<BBTimerComponent>();
            Unit unit = dialogueComponent.GetParent<Unit>();

            TimelinePlayer timelinePlayer = unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<TimelinePlayer>();
            RuntimePlayable runtimePlayable = timelinePlayer.RuntimeimePlayable;

            for (int i = 0; i <= runtimePlayable.ClipMaxFrame(); i++)
            {
                Log.Warning(i.ToString());
                await bbTimer.WaitAsync(1, token);
                if (token.IsCancel()) return Status.Failed;
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}