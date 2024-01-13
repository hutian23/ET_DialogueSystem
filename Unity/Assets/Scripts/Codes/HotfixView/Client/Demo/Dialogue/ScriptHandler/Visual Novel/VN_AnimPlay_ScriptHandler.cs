using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VN_AnimPlay_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_AnimPlay";
        }

        public override async ETTask Handle(Unit unit, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_AnimPlay\s+ch\s*=\s*(\w+)\s*clip\s*=\s*(\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string character = match.Groups[1].Value;
            string clipName = match.Groups[2].Value;

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