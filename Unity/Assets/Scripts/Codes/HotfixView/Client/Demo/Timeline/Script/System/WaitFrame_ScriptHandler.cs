using System.Text.RegularExpressions;

namespace ET.Client
{
    public class WaitFrame_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "WaitFrame";
        }

        //WaitFrame: 3;
        public override async ETTask<Status> Handle(ScriptParser parser, ScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "WaitFrame: (?<frame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScriptMatchError(data.opLine);
                return Status.Failed;
            }

            int.TryParse(match.Groups["frame"].Value, out int frame);
            await parser.GetParent<TimelineComponent>().GetParent<Unit>().GetComponent<BBTimerComponent>().WaitAsync(frame, token);
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}