using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(TargetCheckComponent))]
    public class Function_RegistLoseTargetCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistLoseTargetCallback";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistLoseTargetCallback: (?<GroupName>\w+), (?<FunctionName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TargetCheckComponent targetCheck = parser.GetComponent<TargetCheckComponent>();
            targetCheck.loseTargetCallback_Index = parser.GetFunctionPointer(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value);


            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}