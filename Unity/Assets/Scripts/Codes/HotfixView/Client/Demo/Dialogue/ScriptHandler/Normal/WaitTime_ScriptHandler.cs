using System.Text.RegularExpressions;

namespace ET.Client
{
    public class WaitTime_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "WaitTime";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"WaitTime\s+(\d+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            int.TryParse(match.Groups[1].Value, out int waitTime);
            await TimerComponent.Instance.WaitAsync(waitTime, token);
        }
    }
}