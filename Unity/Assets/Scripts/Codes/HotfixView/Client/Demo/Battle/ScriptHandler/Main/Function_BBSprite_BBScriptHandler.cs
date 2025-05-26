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
            
            //1. 相关组件
            Unit unit = parser.GetParent<Unit>();
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BehaviorInfo behaviorInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());
            TimelinePlayer timelinePlayer = timelineComponent.GetTimelinePlayer();    
            
            //2. 查询Timeline
            BBTimeline timeline = timelinePlayer.GetTimeline(behaviorInfo.behaviorName);
            if (timeline == null)
            {
                Log.Error($"not found timeline name: {behaviorInfo.behaviorName}");
                return Status.Failed;
            }
            //3. 懒加载，只有在调用BBScript、LoopAnim等Timeline模块指令时，才会创建PlayableGraph
            if (timelinePlayer.RuntimePlayable == null || timelinePlayer.RuntimePlayable.timeline != timeline)
            {
                timelineComponent.GetTimelinePlayer().Init(timeline);
            }
            
            //4. 根据SpriteName找到关键帧，并跳转到指定帧
            RuntimePlayable runtimePlayable = timelinePlayer.RuntimePlayable;
            foreach (RuntimeTrack runtimeTrack in runtimePlayable.runtimeTracks)
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