using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CancelSubCoroutine_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CancelSubCoroutine";
        }

        //CancelSubCoroutine: 'GatlingWindow';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine,@"CancelSubCoroutine: '(?<Function>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string subCoroutineName = match.Groups["Function"].Value;
            parser.CancelSubCoroutine(subCoroutineName);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}