using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_ApplyRotation_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ApplyRotation";
        }

        //ApplyRotation: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ApplyRotation: (?<Apply>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            parser.RemoveComponent<RotationComponent>();
            if (match.Groups["Apply"].Value.Equals("true"))
            {
                parser.AddComponent<RotationComponent>(true);
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}