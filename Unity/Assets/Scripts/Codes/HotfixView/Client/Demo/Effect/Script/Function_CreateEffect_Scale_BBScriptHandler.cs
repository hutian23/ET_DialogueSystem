using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_CreateEffect_Scale_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CreateEffect_Scale";
        }

        // CreateEffect_Scale: ScaleX, ScaleY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CreateEffect_Scale: (?<scaleX>.*?), (?<scaleY>.*?);");
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
            
            Unit effect = Root.Instance.Get(parser.GetParam<long>("CreateEffect_UnitId")) as Unit;
            effect.GetComponent<GameObjectComponent>().GameObject.transform.localScale = new Vector3(scaleX / 10000f, scaleY / 10000f, 1f);

            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}