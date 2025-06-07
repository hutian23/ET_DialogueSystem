using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_SkillVFXPosition_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillVFXPosition";
        }

        //SkillVFXPosition: LocalPosX, LocalPosY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SkillVFXPosition: (?<posX>.*?), (?<posY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["posX"].Value, out long posX) ||
                !long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            //1. Get Unit
            GameObject go = parser.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            go.transform.localPosition = new Vector3(posX, posY, 0) / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}