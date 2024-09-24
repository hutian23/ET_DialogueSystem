using System.Text.RegularExpressions;

namespace ET.Client
{
    public class DebugLog_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Log";
        }

        //Log: 'Hello world';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "Log: '(?<Info>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            Log.Debug(match.Groups["Info"].Value);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}