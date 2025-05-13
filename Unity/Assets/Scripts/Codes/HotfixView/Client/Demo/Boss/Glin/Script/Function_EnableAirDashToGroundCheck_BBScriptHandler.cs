using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableAirDashToGroundCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableAirDashToGroundCheck";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableAirDashToGroundCheck: (?<Active>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            parser.RemoveComponent<AirDashToGroundComponent>();
            if (!match.Groups["Active"].Value.Equals("true")) return Status.Success;
            
            parser.AddComponent<AirDashToGroundComponent>();
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}