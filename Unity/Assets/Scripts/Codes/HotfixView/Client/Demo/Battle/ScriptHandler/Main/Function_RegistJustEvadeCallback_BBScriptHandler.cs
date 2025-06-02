using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(JustEvadeCheck))]
    public class Function_RegistJustEvadeCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistJustEvadeCallback";
        }

        // RegistJustEvadeCallback: GroupName, FunctionName;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistJustEvadeCallback: (?<GroupName>\w+), (?<FunctionName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            JustEvadeCheck justEvade = parser.GetComponent<JustEvadeCheck>();
            justEvade.functionIndex = parser.GetFunctionPointer(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}