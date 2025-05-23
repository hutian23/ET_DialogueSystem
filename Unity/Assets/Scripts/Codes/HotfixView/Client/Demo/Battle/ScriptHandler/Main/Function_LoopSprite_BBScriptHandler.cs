using System.Linq;
using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(LoopAnimComponent))]
    public class Function_LoopSprite_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "LoopSprite";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "LoopSprite: (?<Sprite>.*?), (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["WaitFrame"].Value, out int waitFrame))
            {
                Log.Error($"cannot format {match.Groups["WaitFrame"].Value} to int!!");
                return Status.Failed;
            }
            if (waitFrame <= 0)
            {
                Log.Error($"sprite must at least 1 frame!!!");
                return Status.Failed;
            }
            
            TimelineComponent timelineComponent = parser.GetParent<Unit>().GetComponent<TimelineComponent>();
            LoopAnimComponent loopAnim = parser.GetComponent<LoopAnimComponent>();
            
            RuntimePlayable runtimePlayable = timelineComponent.GetTimelinePlayer().RuntimePlayable;
            foreach (RuntimeTrack runtimeTrack in runtimePlayable.RuntimeTracks)
            {
                if (runtimeTrack.Track is not BBEventTrack eventTrack) continue;
                if (eventTrack.Name.Equals("Marker"))
                {
                    EventInfo info = eventTrack.EventInfos.FirstOrDefault(info => info.keyframeName.Equals(match.Groups["Sprite"].Value));
                    if (info == null)
                    {
                        Log.Error($"not found marker:{match.Groups["Sprite"].Value}");
                        return Status.Failed;
                    }
                    
                    loopAnim.spriteQueue.Enqueue(new LoopSpriteDef(){spriteFrame = info.frame, waitFrame = waitFrame});       
                }
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}