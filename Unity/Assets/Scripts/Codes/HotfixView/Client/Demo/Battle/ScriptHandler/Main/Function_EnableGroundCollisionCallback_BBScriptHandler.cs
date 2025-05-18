using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(GroundCollisionCallback))]
    public class Function_EnableGroundCollisionCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGroundCollisionCallback";
        }

        //EnableGroundCollisionCallback: Active, GroupName, FunctionName;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableGroundCollisionCallback: (?<Active>\w+), (?<GroupName>\w+), (?<FunctionName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            parser.RemoveComponent<GroundCollisionCallback>();
            if (!match.Groups["Active"].Value.Equals("true")) return Status.Success;
            
            int funcIndex = parser.GetFunctionPointer(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value);
            GroundCollisionCallback callback = parser.AddComponent<GroundCollisionCallback>();
            callback.functionIndex = funcIndex;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}