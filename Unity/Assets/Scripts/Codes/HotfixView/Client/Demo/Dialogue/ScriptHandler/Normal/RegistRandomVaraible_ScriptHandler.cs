using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (DialogueComponent))]
    public class RegistRandomVaraibleDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistRandomVariable";
        }

        //RegistRandomVariable min = 10 max = 100;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();

            Match match = Regex.Match(line, @"RegistRandomVariable min = (?<min>\w+) max = (?<max>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            int.TryParse(match.Groups["min"].Value, out int min);
            int.TryParse(match.Groups["max"].Value, out int max);
            int value = Random.Range(min, max + 1);
            
            dialogueComponent.RemoveSharedVariable("Random");
            dialogueComponent.Variables.Add(new SharedVariable() { name = "Random", value = value });
            await ETTask.CompletedTask;
        }
    }
}