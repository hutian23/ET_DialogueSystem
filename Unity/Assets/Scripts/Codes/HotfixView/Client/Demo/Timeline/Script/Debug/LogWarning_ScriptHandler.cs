using System.Text.RegularExpressions;

namespace ET.Client
{
    public class LogWarning_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "LogWarning";
        }

        //LogWarning: 'Hello world';
        public override async ETTask<Status> Handle(ScriptParser parser, ScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "LogWarning: '(?<Info>.*?)';");
            if (!match.Success)
            {
                ScriptHelper.ScriptMatchError(data.opLine);
                return Status.Failed;
            }

            Log.Warning(match.Groups["Info"].Value);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}