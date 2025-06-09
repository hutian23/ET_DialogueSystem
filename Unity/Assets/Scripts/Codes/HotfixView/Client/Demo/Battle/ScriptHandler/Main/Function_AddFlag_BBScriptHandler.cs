using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_AddFlag_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AddFlag";
        }

        // AddFlag: Hit;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AddFlag: (?<Flag>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            if(parser.ContainParam($"Flag_{match.Groups["Flag"].Value}")) return Status.Success;
            
            parser.TryRemoveParam($"Flag_{match.Groups["Flag"].Value}");
            parser.RegistParam($"Flag_{match.Groups["Flag"].Value}", true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}