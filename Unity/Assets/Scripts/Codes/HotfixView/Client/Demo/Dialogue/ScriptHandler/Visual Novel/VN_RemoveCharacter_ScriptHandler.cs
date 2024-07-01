using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class VnRemoveCharacterDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RemoveCharacter";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_RemoveCharacter ch = (?<ch>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            GameObject go = new GameObject();
            
            string characterName = match.Groups["ch"].Value;
            CharacterManager manager = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>();
            manager?.RemoveCharacter(characterName);
            await ETTask.CompletedTask;
        }
    }
}