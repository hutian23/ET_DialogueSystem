using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_VFX_LocalPosition_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VFX_LocalPosition";
        }

        //EffectPos: 1000, 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"VFX_LocalPosition: (?<posX>.*?), (?<posY>.*?);");
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
            Unit caster = parser.GetParent<Unit>();
            Unit effect = Root.Instance.Get(parser.GetParam<long>("VFX_UnitId")) as Unit;
            
            //2. Init Position
            b2Body body = b2WorldManager.Instance.GetBody(caster.InstanceId);

            Vector2 position = body.GetPosition().ToUnityVector2();
            Vector2 targetPos = position + new Vector2(posX * body.GetFlip(), posY) / 10000f;
            effect.GetComponent<GameObjectComponent>().GameObject.transform.position = targetPos;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}