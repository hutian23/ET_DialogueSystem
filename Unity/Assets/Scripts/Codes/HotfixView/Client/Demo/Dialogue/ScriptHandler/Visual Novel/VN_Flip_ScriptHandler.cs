using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VnFlipDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_Flip";
        }

        // VN_Flip ch = Celika type = Right;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_Flip ch = (?<ch>\w+) type = (?<type>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string chValue = match.Groups["ch"].Value;
            Unit character = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>().GetCharacter(chValue);

            string typeValue = match.Groups["type"].Value;
            switch (typeValue)
            {
                case "Left":
                    character.SetFac(-1);
                    break;
                case "Right":
                    character.SetFac(1);
                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}