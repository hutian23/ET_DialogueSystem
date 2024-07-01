using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VnCharacterNameDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_CharacterName";
        }

        // VN_CharacterName name = hutian;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_CharacterName name = (?<name>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            DlgDialogue dlgDialogue = unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            dlgDialogue.RefreshCharacterName(match.Groups["name"].Value);
            await ETTask.CompletedTask;
        }
    }
}