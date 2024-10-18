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

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            RuntimePlayable runtimePlayable = timelineComponent.GetTimelinePlayer().RuntimeimePlayable;
            
            foreach (RuntimeTrack runtimeTrack in runtimePlayable.RuntimeTracks)
            {
                if(runtimeTrack.Track is not BBEventTrack eventTrack) continue;
                if (eventTrack.Name.Equals("Marker"))
                {
                    EventInfo info = GetInfo(eventTrack, marker);
                    if (info == null)
                    {
                        Log.Error($"not found marker:{marker}");
                    }

                    timelineComponent.Evaluate(info.frame);
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