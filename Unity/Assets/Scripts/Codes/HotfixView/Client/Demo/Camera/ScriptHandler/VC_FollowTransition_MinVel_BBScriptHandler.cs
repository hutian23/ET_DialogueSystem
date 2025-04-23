using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_FollowTransition_MinVel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowTransition_MinVel";
        }

        // 根据玩家的速度方向进行镜头偏移的调整, 设置开始调整镜头偏移的最小速度(标量）
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_FollowTransition_MinVel: (?<MinVel>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["MinVel"].Value, out long minVel))
            {
                Log.Error($"cannot format {match.Groups["MinVel"].Value} to long!!");
                return Status.Failed;
            }

            parser.TryRemoveParam("VC_FollowTransition_MinVel");
            parser.RegistParam("VC_FollowTransition_MinVel", minVel / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}