using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_FollowOffset_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowOffset";
        }

        //VC_
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_FollowOffset: (?<OffsetX>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["OffsetX"].Value, out long offsetX))
            {
                Log.Error($"cannot format {match.Groups["OffsetX"]} to long!");
                return Status.Failed;
            }

            parser.TryRemoveParam("VC_Follow_Offset");
            parser.TryRemoveParam("VC_Follow_CurrentOffset");
            parser.RegistParam("VC_Follow_Offset", offsetX / 100f);
            parser.RegistParam("VC_Follow_CurrentOffset", offsetX / 100f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}