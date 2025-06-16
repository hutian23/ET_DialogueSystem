using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_VFX_LocalAngle_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VFX_LocalAngle";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {  
            Match match = Regex.Match(data.opLine, @"VFX_LocalAngle: (?<angle>.*?);");
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
            Unit caster = parser.GetParent<Unit>();
            Unit effect = Root.Instance.Get(parser.GetParam<long>("VFX_UnitId")) as Unit;

            GameObject parent = caster.GetComponent<GameObjectComponent>().GameObject;
            GameObject go = effect.GetComponent<GameObjectComponent>().GameObject;
            go.transform.rotation = parent.transform.rotation * Quaternion.Euler(0, 0, angle / 10000f); // Parent局部空间中的旋转 ---> 世界空间的旋转
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}