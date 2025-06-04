using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_CreateEffect_Flip_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CreateEffect_Flip";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CreateEffect_Flip: (?<Flip>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            
            Unit caster = parser.GetParent<Unit>();
            Unit effect = Root.Instance.Get(parser.GetParam<long>("CreateEffect_UnitId")) as Unit;
            b2Body body = b2WorldManager.Instance.GetBody(caster.InstanceId);
            GameObject go = effect.GetComponent<GameObjectComponent>().GameObject;

            Vector2 scale = go.transform.localScale;
            go.transform.localScale = new Vector3(scale.x * body.GetFlip(), scale.y, 1);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}