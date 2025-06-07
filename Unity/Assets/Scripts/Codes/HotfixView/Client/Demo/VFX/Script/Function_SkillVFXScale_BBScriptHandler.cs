using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_SkillVFXScale_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillVFXScale";
        }

        // SkillVFXScale: scaleX, scaleY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SkillVFXScale: (?<scaleX>.*?), (?<scaleY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["scaleX"].Value, out long scaleX) ||
                !long.TryParse(match.Groups["scaleY"].Value, out long scaleY))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            GameObject go = parser.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            go.transform.localScale = new Vector3(scaleX / 10000f, scaleY / 10000f, 1);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}