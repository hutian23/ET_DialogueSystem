using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    public class Timeline_Sprite_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Marker";
        }

        //Marker: 'Rg00_1',3;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "Marker: '(?<Sprite>.*?)', (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string marker = match.Groups["Sprite"].Value;

            Unit unit = parser.GetParent<DialogueComponent>().GetParent<Unit>();
            TimelinePlayer timelinePlayer = unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<TimelinePlayer>();
            BBTimeline timeline = timelinePlayer.CurrentTimeline;

            timeline.MarkDict.TryGetValue(marker, out MarkerInfo info);
            Log.Warning(info.frame.ToString());

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}