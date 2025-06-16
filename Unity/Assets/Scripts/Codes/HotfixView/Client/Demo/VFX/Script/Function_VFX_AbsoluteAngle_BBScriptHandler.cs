using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_VFX_AbsoluteAngle_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VFX_AbsoluteAngle";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"VFX_AbsoluteAngle: (?<angle>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["angle"].Value, out long angle))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            //1. Get Unit
            Unit effect = Root.Instance.Get(parser.GetParam<long>("VFX_UnitId")) as Unit;
            effect.GetComponent<GameObjectComponent>().GameObject.transform.eulerAngles = new Vector3(0, 0, angle / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}