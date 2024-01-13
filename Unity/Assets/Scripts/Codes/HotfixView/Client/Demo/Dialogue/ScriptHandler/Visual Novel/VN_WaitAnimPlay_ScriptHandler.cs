using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VN_WaitAnimPlay_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_WaitAnimPlay";
        }

        public override async ETTask Handle(Unit unit, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_WaitAnimPlay\s+ch\s*=\s*(\w+)\s*clip\s*=\s*(\w+)\s*time\s*=\s*(\d+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string character = match.Groups[1].Value;
            string clipName = match.Groups[2].Value;
            int.TryParse(match.Groups[3].Value, out int time);

            CharacterManager characterManager = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>();
            if (characterManager == null)
            {
                Log.Error($"请添加characterManager");
                return;
            }

            Unit ch = characterManager.GetCharacter(character);
            await ch.WaitAnimAsync(clipName, time, token);
        }
    }
}