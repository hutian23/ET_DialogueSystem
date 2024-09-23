using System.Text.RegularExpressions;

namespace ET.Client
{
    public class WaitFrame_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WaitFrame";
        }

        //WaitFrame: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "WaitFrame: (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            int.TryParse(match.Groups["WaitFrame"].Value, out int frame);
            // await parser.GetParent<TimelineComponent>().GetParent<Unit>().GetComponent<BBTimerComponent>().WaitAsync(frame, token);
            // Log.Warning((parser.GetParent<TimelineComponent>()==null).ToString());
            Log.Warning(frame.ToString());
            await ETTask.CompletedTask;
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}