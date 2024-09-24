using System.Text.RegularExpressions;

namespace ET.Client
{
    public class StopCoroutine_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "StopCoroutine";
        }

        //StopCoroutine: 'Test1';
        public override async ETTask<Status> Handle(ScriptParser parser, ScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "StopCoroutine: '(?<CoroutineName>.*?)';");
            if (!match.Success)
            {
                ScriptHelper.ScriptMatchError(data.opLine);
                return Status.Failed;
            }
            parser.StopSubCoroutine(match.Groups["CoroutineName"].Value);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}