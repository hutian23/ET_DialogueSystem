using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    public class Function_SkillVFXSprite_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillVFXSprite";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "SkillVFXSprite: (?<Sprite>.*?), (?<WaitFrame>.*?);");
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
            
            //1. 层级 Caster ---> Parser ---> SkillVFXManager ---> VFX(Unit)
            Unit unit = parser.GetParent<Unit>();
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BBTimerComponent casterTimer = unit.GetParent<SkillVFXManager>()
                    .GetParent<BBParser>()
                    .GetParent<Unit>()
                    .GetComponent<BBTimerComponent>();
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

            int targetFrame = timelineComponent.GetTargetFrame(match.Groups["Sprite"].Value);
            timelineComponent.Evaluate(targetFrame);
            
            await casterTimer.WaitAsync(waitFrame, token);
            
            return token.IsCancel() ? Status.Failed : Status.Success;
        }
    }
}