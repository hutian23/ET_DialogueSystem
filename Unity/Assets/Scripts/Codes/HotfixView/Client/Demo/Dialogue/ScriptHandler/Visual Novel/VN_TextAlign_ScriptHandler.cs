using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class VN_TextAlign_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_TextAlign";
        }

        //VN_TextAlign type = MiddleCenter;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_TextAlign type = (?<type>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            var type = match.Groups["type"].Value;
            if (!Enum.TryParse(type, out TextAnchor anchor))
            {
                Log.Error($"not found TextAnchor: {type}");
                return;
            }

            DlgDialogue dlgDialogue = unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            dlgDialogue.View.E_TextText.alignment = anchor;
            await ETTask.CompletedTask;
        }
    }
}