using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VN_RegisterCharacter_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RegisterCharacter";
        }

        public override async ETTask Handle(Unit unit, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_RegisterCharacter ch = (?<ch>\w+) unitId = (?<unitId>\d+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string characterName = match.Groups["ch"].Value;
            int.TryParse(match.Groups["unitId"].Value, out int unitId);

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            CharacterManager characterManager = dialogueComponent.GetComponent<CharacterManager>() ?? dialogueComponent.AddComponent<CharacterManager>();
            await characterManager.RegisterCharacter(characterName, unitId);
        }
    }
}