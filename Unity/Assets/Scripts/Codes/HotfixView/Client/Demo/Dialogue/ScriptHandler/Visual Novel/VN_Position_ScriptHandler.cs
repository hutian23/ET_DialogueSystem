using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class VN_Position_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_Position";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_Position ch = (?<ch>\w+)(?: type = (?<type>\w+))?(?: position = \((?<x>-?\d+),(?<y>-?\d+)\))?;");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string chValue = match.Groups["ch"].Value;
            Unit character = unit.GetComponent<DialogueComponent>().GetComponent<CharacterManager>().GetCharacter(chValue);

            if (match.Groups["type"].Success)
            {
                switch (match.Groups["type"].Value)
                {
                    case "Middle":
                        character.SetPosition(VN_Position.Middle);
                        break;
                    case "Left":
                        character.SetPosition(VN_Position.Left);
                        break;
                    case "Right":
                        character.SetPosition(VN_Position.Right);
                        break;
                }
            }
            else if (match.Groups["x"].Success && match.Groups["y"].Success)
            {
                float.TryParse(match.Groups["x"].Value, out float xPos);
                float.TryParse(match.Groups["y"].Value, out float yPos);
                character.SetPosition(new Vector2(xPos, yPos));
            }
            else
            {
                Log.Error("type 和 position至少需要一个参数!");
                return;
            }
            await ETTask.CompletedTask;
        }
    }
}