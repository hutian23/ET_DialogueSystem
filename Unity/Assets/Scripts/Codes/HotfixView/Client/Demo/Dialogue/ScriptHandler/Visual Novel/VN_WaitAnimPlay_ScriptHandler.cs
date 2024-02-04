using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VN_WaitAnimPlay_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_WaitAnimPlay";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_WaitAnimPlay ch = (?<ch>\w+) clip = (?<clip>\w+) time = (?<time>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string character = match.Groups["ch"].Value;
            string clipName = match.Groups["clip"].Value;
            int.TryParse(match.Groups["time"].Value, out int time);

            CharacterManager characterManager = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>();
            Unit ch = characterManager.GetCharacter(character);
            await ch.WaitAnimAsync(clipName, time, token);
        }
    }
}