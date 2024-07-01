using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VnShowCharacterDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_ShowCharacter";
        }

        //VN_ShowCharacter ch = Skye;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_ShowCharacter ch = (?<name>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            CharacterManager characterManager = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>();
            string characterName = match.Groups["name"].Value;
            characterManager.ShowCharacter(characterName, true);
            await ETTask.CompletedTask;
        }
    }
}