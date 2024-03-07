using System.Text.RegularExpressions;

namespace ET.Client
{
    public class DebugLogWarning_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "LogWarning";
        }

        //LogWarning: 'Hello world';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "LogWarning: '(?<Info>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            Log.Warning(match.Groups["Info"].Value);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}