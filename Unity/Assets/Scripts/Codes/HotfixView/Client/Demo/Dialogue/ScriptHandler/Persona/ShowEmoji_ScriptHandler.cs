using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class ShowEmoji_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "ShowEmoji";
        }

        public override async ETTask Handle(Unit unit, string line, ETCancellationToken token)
        {
            Match match1 = Regex.Match(line, @"ShowEmoji\s+""([^""]*)""");
            string icon = match1.Groups[1].Value;

            Sprite sprite = await IconHelper.LoadIconSpriteAsync("", icon);
            EmojiComponent emoji = unit.GetComponent<DialogueComponent>().AddComponent<EmojiComponent>();
            Log.Warning(sprite + " " + emoji);
            await ETTask.CompletedTask;
        }
    }
}