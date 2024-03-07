using System.Text.RegularExpressions;

namespace ET.Client
{
    public class WaitTime_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WaitFrame";
        }

        //WaitFrame frame = 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"WaitFrame frame = (?<frame>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            int.TryParse(match.Groups["frame"].Value, out int frame);
            await parser.GetParent<DialogueComponent>().GetComponent<BBTimerComponent>().WaitAsync(frame, token);
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}