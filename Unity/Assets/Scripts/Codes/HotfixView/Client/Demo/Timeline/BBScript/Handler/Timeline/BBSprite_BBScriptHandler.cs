using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    public class BBSprite_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BBSprite";
        }

        //BBSprite: 'Rg00_1',3;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "BBSprite: '(?<Sprite>.*?)', (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string marker = match.Groups["Sprite"].Value;
            int.TryParse(match.Groups["WaitFrame"].Value, out int waitFrame);
            
            Unit unit = parser.GetParent<DialogueComponent>().GetParent<Unit>();
            
            //Evaluate playableGraph
            TimelinePlayer timelinePlayer = unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<TimelinePlayer>();
            BBTimeline timeline = timelinePlayer.CurrentTimeline;
            // timeline.Marks.TryGetValue(marker, out MarkerInfo info);
            RuntimePlayable runtimePlayable = timelinePlayer.RuntimeimePlayable;
            // runtimePlayable.Evaluate(info.frame);
            
            //wait time
            BBTimerComponent timerComponent = parser.GetParent<DialogueComponent>().GetComponent<BBTimerComponent>();
            await timerComponent.WaitAsync(waitFrame, token);
            if (token.IsCancel()) return Status.Failed;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}