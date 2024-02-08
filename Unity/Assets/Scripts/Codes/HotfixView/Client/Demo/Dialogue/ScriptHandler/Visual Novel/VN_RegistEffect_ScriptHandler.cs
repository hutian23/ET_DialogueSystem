using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class VN_RegistEffect_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RegistEffect";
        }

        //VN_RegistEffect name = Hold_it prefabName = HoldIt position = (3,3);
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_RegistEffect name = (?<name>\w+) prefabName = (?<prefabName>\w+)?(?: position = \((?<x>-?\d+),(?<y>-?\d+)\))?;");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }
            
            EffectManager manager = unit.GetComponent<DialogueComponent>().GetComponent<EffectManager>() == null
                    ? unit.GetComponent<DialogueComponent>().AddComponent<EffectManager>()
                    : unit.GetComponent<DialogueComponent>().GetComponent<EffectManager>();
            
            GameObject effect = await manager.RegistEffect(match.Groups["name"].Value, match.Groups["prefabName"].Value);
            
            float.TryParse(match.Groups["x"].Value, out float xPos);
            float.TryParse(match.Groups["y"].Value, out float yPos);
            effect.transform.position = new Vector2(xPos , yPos);
        }
    }
}