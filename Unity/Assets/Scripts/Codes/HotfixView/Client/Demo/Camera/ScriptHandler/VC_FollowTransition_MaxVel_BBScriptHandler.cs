using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_FollowTransition_MaxVel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowTransition_MaxVel";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_FollowTransition_MaxVel: (?<MaxVel>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["MaxVel"].Value, out long maxVel))
            {
                Log.Error($"cannot format {match.Groups["MaxVel"].Value} to long!!");
                return Status.Failed;
            }

            parser.TryRemoveParam("VC_FollowTransition_MaxVel");
            parser.RegistParam("VC_FollowTransition_MaxVel", maxVel / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}