using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VN_RemoveCharacter_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RemoveCharacter";
        }

        public override async ETTask Handle(Unit unit, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_RemoveCharacter\s+ch\s*=\s*(\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string characterName = match.Groups[1].Value;
            CharacterManager manager = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>();
            if (manager == null)
            {
                Log.Error("请添加characterManager");
                return;
            }

            manager.RemoveCharacter(characterName);
            await ETTask.CompletedTask;
        }
    }
}