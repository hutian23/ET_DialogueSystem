using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_SpawnADust_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SpawnADust";
        }

        //SpawnADust: PosX, PosY, ScaleX, ScaleY, Angle;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "SpawnADust: (?<posX>.*?), (?<posY>.*?), (?<scaleX>.*?), (?<scaleY>.*?), (?<angle>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["posX"].Value, out long posX) || 
                !long.TryParse(match.Groups["posY"].Value, out long posY) ||
                !long.TryParse(match.Groups["scaleX"].Value, out long scaleX) ||
                !long.TryParse(match.Groups["scaleY"].Value, out long scaleY) ||
                !long.TryParse(match.Groups["angle"].Value, out long angle))
            {
                Log.Error($"cannot format posX / posY to long");
                return Status.Failed;
            }
            
            //1. 创建effect unit
            Unit effect = BulletManager.Instance.AddChild<Unit, int>(1001);
            Unit caster = parser.GetParent<Unit>();
            
            //2. 添加组件
            GameObject parent = caster.GetComponent<GameObjectComponent>().GameObject;
            GameObject go = GameObjectPoolHelper.GetObjectFromPool("ADust");
            effect.AddComponent<GameObjectComponent>().GameObject = go;
            effect.AddComponent<BBParser>();
            
            //3. 初始化
            go.transform.position = parent.transform.TransformPoint(new Vector3(posX, posY, 0) / 10000f);
            go.transform.rotation = parent.transform.rotation * Quaternion.Euler(0, 0, angle / 10000f); // 将Parent局部空间中的旋转(0, 0, 90)转换至世界空间的旋转
            go.transform.localScale = new Vector3(scaleX / 10000f, scaleY / 10000f, 1f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}