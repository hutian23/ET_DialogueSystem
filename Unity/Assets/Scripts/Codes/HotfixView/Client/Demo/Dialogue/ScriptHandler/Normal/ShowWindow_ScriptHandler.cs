using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class ShowWindowDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "ShowWindow";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"ShowWindow type = (?<WindowType>\w+);");
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

            token.Add(() => { unit.ClientScene().GetComponent<UIComponent>().UnLoadWindow(windowID); });
            await unit.ClientScene().GetComponent<UIComponent>().ShowWindowAsync(windowID);
        }
    }
}