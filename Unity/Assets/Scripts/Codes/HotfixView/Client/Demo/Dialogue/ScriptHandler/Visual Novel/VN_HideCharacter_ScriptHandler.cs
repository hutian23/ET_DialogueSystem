using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VnHideCharacterDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_HideCharacter";
        }

        //VN_HideCharacter ch = Skye;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_HideCharacter ch = (?<name>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            CharacterManager characterManager = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>();
            string characterName = match.Groups["name"].Value;
            characterManager.ShowCharacter(characterName, false);

            await ETTask.CompletedTask;
        }
    }
}