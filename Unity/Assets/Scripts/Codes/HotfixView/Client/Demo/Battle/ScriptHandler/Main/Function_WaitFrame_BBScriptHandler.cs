using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_WaitFrame_BBScriptHandler: BBScriptHandler
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
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            int.TryParse(match.Groups["WaitFrame"].Value, out int frame);

            BBTimerComponent bbTimer = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();
            await bbTimer.WaitAsync(frame, token);
            
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}