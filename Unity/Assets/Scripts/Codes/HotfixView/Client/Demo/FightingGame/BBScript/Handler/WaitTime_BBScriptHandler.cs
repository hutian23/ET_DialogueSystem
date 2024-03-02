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
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            Match match = Regex.Match(opCode, @"WaitFrame frame = (?<frame>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }

            int.TryParse(match.Groups["frame"].Value, out int frame);
            await unit.GetComponent<DialogueComponent>().GetComponent<BBTimerComponent>().WaitAsync(frame, token);
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}