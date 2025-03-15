using System.Text.RegularExpressions;

namespace ET.Client
{
    public class GCOption_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GCOption";
        }

        //GCOption: Rg_Cancel;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"GCOption: (?<Option>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            HashSetComponent<string> options = parser.GetParam<HashSetComponent<string>>("GatlingCancel_Options");
            options.Add(match.Groups["Option"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}