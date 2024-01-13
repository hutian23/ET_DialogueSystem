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
            Match match = Regex.Match(line, @"VN_RegisterCharacter\s+name\s*=\s*(\w+)\s*unitId\s*=\s*(\d+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string characterName = match.Groups[1].Value;
            int.TryParse(match.Groups[2].Value, out int unitId);

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            CharacterManager characterManager = dialogueComponent.GetComponent<CharacterManager>() ?? dialogueComponent.AddComponent<CharacterManager>();
            await characterManager.RegisterCharacter(characterName, unitId);
        }
    }
}