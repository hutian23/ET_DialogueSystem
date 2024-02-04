using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VN_AnimPlay_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_AnimPlay";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_AnimPlay ch = (?<ch>\w+) clip = (?<clip>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string character = match.Groups["ch"].Value;
            string clipName = match.Groups["clip"].Value;

            CharacterManager characterManager = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>();
            if (characterManager == null)
            {
                Log.Error("请添加characterManager");
                return;
            }

            Unit ch = characterManager.GetCharacter(character);
            ch.AnimPlay(clipName);
            
            await ETTask.CompletedTask;
        }
    }
}