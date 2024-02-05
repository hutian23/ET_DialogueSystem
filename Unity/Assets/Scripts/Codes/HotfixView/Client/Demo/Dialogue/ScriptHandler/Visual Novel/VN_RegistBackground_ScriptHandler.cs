using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class VN_RegistBackground_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RegistBackground";
        }

        //VN_RegistBackground name = courtRoom;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_RegistBackground name = (?<spriteName>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            Sprite sprite = await IconHelper.LoadIconSpriteAsync("Sprites", match.Groups["spriteName"].Value);

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            Background background = dialogueComponent.GetComponent<Background>() != null? dialogueComponent.GetComponent<Background>() : dialogueComponent.AddComponent<Background>();
            background.ShowBackground(sprite);
            await ETTask.CompletedTask;
        }
    }
}