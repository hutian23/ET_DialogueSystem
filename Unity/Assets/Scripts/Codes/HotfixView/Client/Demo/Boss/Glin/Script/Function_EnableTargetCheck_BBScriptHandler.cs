using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableTargetCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableTargetCheck";
        }

        // EnableTargetCheck: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableTargetCheck: (?<Active>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            parser.RemoveComponent<TargetCancelComponent>();
            if(!match.Groups["Active"].Value.Equals("true")) return Status.Success;
            
            parser.AddComponent<TargetCheckComponent>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}