using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableHardLandCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableHardLandCheck";
        }

        // EnableHardLandCheck: LastFrame, AirVelocity;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableHardLandCheck: (?<LastFrame>.*?), (?<AirVelocity>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["LastFrame"].Value, out int lastFrame) ||
                !long.TryParse(match.Groups["AirVelocity"].Value, out long airVelocity))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            parser.RemoveComponent<HardLandCheckComponent>();
            parser.AddComponent<HardLandCheckComponent, int, float>(lastFrame, -airVelocity / 10000f, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}