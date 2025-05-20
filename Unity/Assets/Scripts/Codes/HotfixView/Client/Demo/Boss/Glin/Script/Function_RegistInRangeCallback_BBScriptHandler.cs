using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(InRangeCheckComponent))]
    public class Function_RegistInRangeCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistInRangeCallback";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistInRangeCallback: (?<GroupName>\w+), (?<FunctionName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            InRangeCheckComponent inRange = parser.GetComponent<InRangeCheckComponent>();
            inRange.inRangeCallbackIndex = parser.GetFunctionPointer(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}