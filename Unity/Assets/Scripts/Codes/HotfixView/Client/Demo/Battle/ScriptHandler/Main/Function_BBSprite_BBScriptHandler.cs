using System.Linq;
using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    public class Function_BBSprite_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BBSprite";
        }

        //BBSprite: Rg00_1,3;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "BBSprite: (?<Sprite>.*?), (?<WaitFrame>.*?);");
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
            
            Unit unit = parser.GetParent<Unit>();
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BehaviorInfo behaviorInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());
            
            //1. 注意，behaviorName和BBPlayableGraph中的TimelineDict.Key对应
            // 只有在调用和Timeline相关的语句时，才会进行playableGraph的更新
            BBTimeline _timeline = timelineComponent.GetTimelinePlayer().GetTimeline(behaviorInfo.behaviorName);
            if (_timeline == null)
            {
                Log.Error($"not found timeline name: {behaviorInfo.behaviorName}");
                return Status.Failed;
            }
            if (timelineComponent.GetTimelinePlayer().RuntimePlayable == null ||
                timelineComponent.GetTimelinePlayer().RuntimePlayable.Timeline != _timeline)
            {
                timelineComponent.GetTimelinePlayer().Init(_timeline);
            }
            
            //2. 找到关键帧, timeline跳转到对应帧
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

                    timelineComponent.Evaluate(info.frame);
                    await bbTimer.WaitAsync(waitFrame, token);
                    return token.IsCancel() ? Status.Failed : Status.Success;
                }
            }
            Log.Error("Not found bbEventTrack: Marker");
            return Status.Failed;
        }
    }
}