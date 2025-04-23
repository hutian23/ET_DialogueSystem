using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_MaxFOV_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_MaxFOV";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_MaxFOV: (?<FOV>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["FOV"].Value, out long fov))
            {
                Log.Error($"cannot format {match.Groups["FOV"].Value} to long");
                return Status.Failed;
            }

            parser.TryRemoveParam("VC_MaxFOV");
            parser.RegistParam("VC_MaxFOV", fov / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}