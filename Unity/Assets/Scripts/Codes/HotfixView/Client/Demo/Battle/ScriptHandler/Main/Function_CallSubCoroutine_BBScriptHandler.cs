using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_CallSubCoroutine_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CallSubCoroutine";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CallSubCoroutine: (?<GroupName>\w+), (?<FunctionName>\w+);");
            parser.Invoke(parser.GetFunctionPointer(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value), token).Coroutine();

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}