using System.Text.RegularExpressions;

namespace ET.Client
{
    public class DebugLog_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Log";
        }

        //Log "Hello world";
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            Match match = Regex.Match(opCode, "\"(.*?)\"");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }

            Log.Debug(match.Groups[1].Value);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}