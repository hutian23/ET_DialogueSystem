using System.Linq;
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

            
            TimelinePlayer timelinePlayer = parser.GetParent<TimelineComponent>().GetTimelinePlayer();
            BBTimerComponent bbTimer = parser.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            BBTimeline timeline = timelinePlayer.CurrentTimeline;
            RuntimePlayable runtimePlayable = timelinePlayer.RuntimeimePlayable;
            foreach (BBTrack track in timeline.Tracks)
            {
                if (track is BBEventTrack { Name: "Marker" } eventTrack)
                {
                    EventInfo info = GetInfo(eventTrack, marker);
                    if (info == null)
                    {
                        Log.Error($"not found marker:{marker}");
                    }

                    runtimePlayable.Evaluate(info.frame);
                    await bbTimer.WaitAsync(waitFrame, token);
                    return token.IsCancel()? Status.Failed : Status.Success;
                }
            }

            Log.Error("Not found bbEventTrack: Marker");
            return Status.Failed;
        }

        private EventInfo GetInfo(BBEventTrack track, string markerName)
        {
            return track.EventInfos.FirstOrDefault(info => info.keyframeName == markerName);
        }
    }
}