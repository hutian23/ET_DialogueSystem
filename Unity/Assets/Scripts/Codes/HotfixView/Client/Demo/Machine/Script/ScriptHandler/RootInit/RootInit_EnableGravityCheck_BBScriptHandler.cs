using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(GravityCheckComponent))]
    public class RootInit_EnableGravityCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGravityCheck";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"EnableGravityCheck: (?<gravity>\w+), (?<maxGravity>\w+), (?<maxFall>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["gravity"].Value, out long gravity) ||
                !long.TryParse(match.Groups["maxGravity"].Value, out long maxGravity) ||
                !long.TryParse(match.Groups["maxFall"].Value, out long maxFall))
            {
                Log.Error($"cannot format {match.Groups["gravity"].Value} to long!!!");
                return Status.Failed;
            }
            
            Unit unit = parser.GetParent<Unit>();

            //2. 组件初始化
            unit.RemoveBuff<GravityCheckComponent>();
            GravityCheckComponent gc = unit.AddBuff<GravityCheckComponent>();
            gc.gravity = gravity / 1000f;
            gc.maxGravity = maxGravity / 1000f;
            gc.maxFall = maxFall / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}