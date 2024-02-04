using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VN_RemoveEmoji_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RemoveEmoji";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_RemoveEmoji ch = (?<ch>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }
            string ch = match.Groups["ch"].Value;
            Unit character = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>().GetCharacter(ch);
            character.RemoveComponent<EmojiComponent>();
            await ETTask.CompletedTask;
        }
    }
}