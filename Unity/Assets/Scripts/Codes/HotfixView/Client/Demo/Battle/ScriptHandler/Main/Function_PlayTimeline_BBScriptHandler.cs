using System;
using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    public class Function_PlayTimeline_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "PlayTimeline";
        }

        //PlayTimeline: 1, 10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "PlayTimeline: (?<StartFrame>.*?), (?<StopFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["StartFrame"].Value, out int _startFrame) ||
                !int.TryParse(match.Groups["StopFrame"].Value, out int _stopFrame))
            {
                Log.Error($"cannot format {match.Groups["StartFrame"].Value} or {match.Groups["StopFrame"].Value} to int!!!");
                return Status.Failed;
            }
                
            Unit unit = parser.GetParent<Unit>();
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BehaviorInfo behaviorInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());

            //1. 更新PlayableGraph
            BBTimeline _timeline = timelineComponent.GetTimelinePlayer().GetTimeline(behaviorInfo.behaviorName);
            timelineComponent.GetTimelinePlayer().Init(_timeline);
            
            //2. 逐帧执行Timeline
            RuntimePlayable playable = timelineComponent.GetTimelinePlayer().RuntimePlayable;
            for (int i = _startFrame; i < Math.Min(_stopFrame, playable.GetMaxFrame()); i++)
            {
                timelineComponent.Evaluate(i);
                await bbTimer.WaitAsync(1, token);
                if (token.IsCancel()) break;
            }

            return token.IsCancel() ? Status.Failed : Status.Success;
        }
    }
}