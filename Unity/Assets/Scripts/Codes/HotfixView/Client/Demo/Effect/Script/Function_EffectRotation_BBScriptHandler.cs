using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_EffectRotation_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EffectRotation";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {  
            Match match = Regex.Match(data.opLine, @"EffectRotation: (?<rotate>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["rotate"].Value, out long rotate))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            //1. Get Unit
            Unit caster = parser.GetParent<Unit>();
            Unit effect = Root.Instance.Get(parser.GetParam<long>("CreateEffect_UnitId")) as Unit;
            
            effect.GetComponent<GameObjectComponent>().GameObject.transform.localEulerAngles = new Vector3(0, 0, rotate / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}