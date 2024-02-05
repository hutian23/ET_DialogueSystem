using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VN_RegistTalker_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RegistTalker";
        }

        //VN_RegistTalker ch = Skye clip = Happy;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_RegistTalker ch = (?<ch>\w+) clip = (?<clip>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string character = match.Groups["ch"].Value;
            string idle_clip = match.Groups["clip"].Value;

            CharacterManager characterManager = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>();
            characterManager.RegistTalker(character, idle_clip);

            await ETTask.CompletedTask;
        }
    }
}