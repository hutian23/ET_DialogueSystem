using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableRepeatedTimerCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableRepeatedTimerCallback";
        }

        //EnableRepeatedTimerCallback: Active, RepeatedFrame, GroupName, FunctionName;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableRepeatedTimerCallback: (?<Active>\w+), (?<RepeatedFrame>.*?), (?<GroupName>\w+), (?<FunctionName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["RepeatedFrame"].Value, out int repeatedFrame))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            parser.RemoveComponent<RepeatedTimerCallback>();
            if (!match.Groups["Active"].Value.Equals("true")) return Status.Success;

            int functionIndex = parser.GetFunctionPointer(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value);
            parser.AddComponent<RepeatedTimerCallback, int, int>(repeatedFrame, functionIndex);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}