using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(GroundCollisionComponent))]
    public class Function_RegistGroundCollisionCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistGroundCollisionCallback";
        }

        //RegistGroundCollisionCallback: GroupName, FunctionName;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistGroundCollisionCallback: (?<GroupName>\w+), (?<FunctionName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            GroundCollisionComponent component = parser.GetComponent<GroundCollisionComponent>();
            component.functionIndex = parser.GetFunctionPointer(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}