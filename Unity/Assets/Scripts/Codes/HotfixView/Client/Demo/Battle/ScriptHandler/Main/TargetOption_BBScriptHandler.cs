using System.Text.RegularExpressions;

namespace ET.Client
{
    public class TargetOption_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TCOption";
        }

        //TargetOption: Rg_Test;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"TCOption: (?<Option>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }

            TargetCancelComponent tc = parser.GetComponent<TargetCancelComponent>();
            tc.Add(match.Groups["Option"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}