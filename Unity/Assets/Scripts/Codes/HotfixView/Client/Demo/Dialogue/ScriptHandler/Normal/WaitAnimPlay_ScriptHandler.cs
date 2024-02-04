using System.Text.RegularExpressions;

namespace ET.Client
{
    public class WaitAnimPlay_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "WaitAnimPlay";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"WaitAnimPlay ""(.*?)"" (\d+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string clipName = match.Groups[1].Value;
            int.TryParse(match.Groups[2].Value, out int animTime);
            await unit.WaitAnimAsync(clipName, animTime, token);
        }
    }
}