using System.Text.RegularExpressions;

namespace ET.Client
{
    // 最小可见范围(Field Of View)
    public class VC_MinFOV_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_MinFOV";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_MinFOV: (?<FOV>.*?);");
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

            parser.TryRemoveParam("VC_MinFOV");
            parser.RegistParam("VC_MinFOV", fov / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}