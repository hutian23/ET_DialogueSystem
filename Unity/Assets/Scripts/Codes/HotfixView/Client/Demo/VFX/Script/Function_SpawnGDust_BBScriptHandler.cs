using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_SpawnGDust_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SpawnGDust";
        }

        //SpawnGDust: PosX, PosY, ScaleX, ScaleY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "SpawnGDust: (?<posX>.*?), (?<posY>.*?), (?<scaleX>.*?), (?<scaleY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["posX"].Value, out long posX) ||
                !long.TryParse(match.Groups["posY"].Value, out long posY) ||
                !long.TryParse(match.Groups["scaleX"].Value, out long scaleX) ||
                !long.TryParse(match.Groups["scaleY"].Value, out long scaleY))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            //1. 创建effect unit
            Unit effect = BattleSceneManager.Instance.AddChild<Unit, int>(1001);
            Unit caster = parser.GetParent<Unit>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(caster.InstanceId);
            
            //2. 添加组件
            GameObject parent = caster.GetComponent<GameObjectComponent>().GameObject;
            GameObject go = GameObjectPoolHelper.GetObjectFromPool("GDust");
            effect.AddComponent<GameObjectComponent>().GameObject = go;
            effect.AddComponent<BBParser>();
            
            //3.  
            go.transform.position = parent.transform.position + new Vector3(b2Body.GetFlip() * posX, posY, 0) / 10000f;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = new Vector3(b2Body.GetFlip() * scaleX / 10000f, scaleY / 10000f, 1f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}