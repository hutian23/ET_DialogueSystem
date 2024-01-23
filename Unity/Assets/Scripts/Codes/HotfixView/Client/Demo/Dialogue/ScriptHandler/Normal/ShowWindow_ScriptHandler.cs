using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class ShowWindow_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "ShowWindow";
        }

        public override async ETTask Handle(Unit unit, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"ShowWindow type = (?<WindowType>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            if(!Enum.TryParse(match.Groups["WindowType"].Value, out WindowID windowID)) return;
            
            token.Add(() => unit.ClientScene().GetComponent<UIComponent>().HideWindow(windowID));
            await unit.ClientScene().GetComponent<UIComponent>().ShowWindowAsync(windowID);
        }
    }
}