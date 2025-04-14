using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableFlip_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableFlip";
        }

        //EnableFlip: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableFlip: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            parser.RemoveComponent<FlipCheckComponent>();
            if (match.Groups["Enable"].Success)
            {
                parser.AddComponent<FlipCheckComponent>();   
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}