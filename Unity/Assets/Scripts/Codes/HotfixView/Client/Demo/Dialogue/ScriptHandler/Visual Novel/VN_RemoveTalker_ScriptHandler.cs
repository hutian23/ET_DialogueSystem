using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VN_RemoveTalker_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RemoveTalker";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_RemoveTalker ch = (?<ch>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string character = match.Groups["ch"].Value;
            CharacterManager characterManager = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>();
            characterManager.RemoveTalker(character);
            
            await ETTask.CompletedTask;
        }
    }
}