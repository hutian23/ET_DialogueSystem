using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class HideWindowDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "HideWindow";
        }

        public override async ETTask Handle(Unit unit,DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"HideWindow type = (?<WindowType>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            if (!Enum.TryParse($"WindowID_{match.Groups["WindowType"].Value}", out WindowID windowID))
            {
                Log.Error($"not found windowID: {match.Groups["WindowType"]}");
                return;
            }

            unit.ClientScene().GetComponent<UIComponent>().HideWindow(windowID);
            await ETTask.CompletedTask;
        }
    }
}