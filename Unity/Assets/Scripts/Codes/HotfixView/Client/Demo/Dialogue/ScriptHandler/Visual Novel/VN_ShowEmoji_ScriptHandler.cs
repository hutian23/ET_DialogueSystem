using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class VN_ShowEmoji_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_ShowEmoji";
        }

        public override async ETTask Handle(Unit unit, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_ShowEmoji ch = (?<ch>\w+) type = (?<type>\w+)(?: position = \((?<x>\d+),(?<y>\d+)\))?");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string chValue = match.Groups["ch"].Value;
            string type = match.Groups["type"].Value;
            string xValue = match.Groups["x"].Success? match.Groups["x"].Value : "N/A";
            string yValue = match.Groups["y"].Success? match.Groups["y"].Value : "N/A";

            Unit character = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>().GetCharacter(chValue);
            character.RemoveComponent<EmojiComponent>();
            await character.AddComponent<EmojiComponent>().SpawnEmoji(type, token);
            if (xValue == "N/A" || yValue == "N/A") return;

            float.TryParse(xValue, out float xPos);
            float.TryParse(yValue, out float yPos);
            var chPos = new Vector2(xPos, yPos);
            character.GetComponent<EmojiComponent>().SetPosition(chPos);
        }
    }
}