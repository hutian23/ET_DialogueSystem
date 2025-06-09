using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableEnhanceInput_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableEnhanceInput";
        }
        
        // EnableEnhanceInput: Active;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableEnhanceInput: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            parser.RemoveComponent<EnhanceInputComponent>();
            if (!match.Groups["Enable"].Value.Equals("true")) return Status.Success;
            
            parser.AddComponent<EnhanceInputComponent>(true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}