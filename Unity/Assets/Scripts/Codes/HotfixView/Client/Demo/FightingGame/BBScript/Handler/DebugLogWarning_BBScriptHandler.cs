using System.Text.RegularExpressions;

namespace ET.Client
{
    public class DebugLogWarning_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "LogWarning";
        }

        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            Match match = Regex.Match(opCode, "\"(.*?)\"");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }

            Log.Warning(match.Groups[1].Value);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}