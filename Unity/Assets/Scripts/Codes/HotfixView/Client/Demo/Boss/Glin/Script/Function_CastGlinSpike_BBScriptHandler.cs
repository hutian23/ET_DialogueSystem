using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_CastGlinSpike_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CastGlinSpike";
        }

        //CastGlinSpike: 地刺数量, 地刺x轴偏移量;
        //CastGlinSpike: 4, 50000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"CastGlinSpike: (?<X>.*?), (?<Y>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["X"].Value, out long x) || 
                !long.TryParse(match.Groups["Y"].Value, out long y))
            {
                Log.Error($"match failed");
                return Status.Failed;
            }

            //1. 创建Unit
            b2Body bodyA = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            Unit unit = BulletManager.Instance.AddChild<Unit, int>(1001);
            
            //2. 添加组件
            GameObject bullet = GameObjectPoolHelper.GetObjectFromPool("GlinSpike");
            unit.AddComponent<GameObjectComponent>().GameObject = bullet;
            unit.AddComponent<BBParser>();

            //3. 设置偏移量
            b2Body bodyB = b2WorldManager.Instance.GetBody(unit.InstanceId);
            bodyB.SetPosition(bodyA.GetPosition() + new System.Numerics.Vector2(x, y) / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}