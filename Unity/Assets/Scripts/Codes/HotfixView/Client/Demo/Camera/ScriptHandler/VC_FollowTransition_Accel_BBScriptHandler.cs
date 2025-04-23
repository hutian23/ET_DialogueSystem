using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_FollowTransition_Accel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowTransition_Accel";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_FollowTransition_Accel: (?<Accel>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["Accel"].Value, out long accel))
            {
                Log.Error($"cannot format {match.Groups["Accel"].Value} to long!!");
                return Status.Failed;
            }

            parser.TryRemoveParam("VC_FollowTransition_Accel");
            parser.RegistParam("VC_FollowTransition_Accel", accel / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}