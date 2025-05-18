using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableWaitFrameCallback_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableWaitFrameCallback";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableWaitFrameCallback: (?<Active>\w+), (?<WaitFrame>.*?), (?<GroupName>\w+), (?<FunctionName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["WaitFrame"].Value, out int waitFrame))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            parser.RemoveComponent<WaitFrameCallback>();
            if (!match.Groups["Active"].Value.Equals("true")) return Status.Success;

            int functionIndex = parser.GetFunctionPointer(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value);
            parser.AddComponent<WaitFrameCallback, int, int>(waitFrame, functionIndex, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}