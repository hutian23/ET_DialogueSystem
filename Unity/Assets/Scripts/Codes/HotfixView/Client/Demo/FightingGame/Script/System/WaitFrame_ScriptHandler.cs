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
        public override async ETTask<Status> Handle(Unit unit, ScriptData data, ETCancellationToken token)
        {
            BBTimerComponent timer = unit.GetComponent<TimelineComponent>().GetComponent<BBTimerComponent>();
            Match match = Regex.Match(data.opLine, "WaitFrame: (?<frame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScriptMatchError(data.opLine);
                return Status.Failed;
            }

            int.TryParse(match.Groups["frame"].Value, out int frame);
            await timer.WaitAsync(frame, token);
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}