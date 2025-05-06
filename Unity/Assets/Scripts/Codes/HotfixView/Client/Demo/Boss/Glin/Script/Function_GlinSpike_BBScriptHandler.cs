using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_GlinSpike_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GlinSpike";
        }

        //GlinSpike: 地刺数量, 地刺x轴偏移量;
        //GlinSpike: 4, 50000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"GlinSpike: (?<Count>.*?), (?<Offset>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["Count"].Value, out int count) || 
                !long.TryParse(match.Groups["Offset"].Value, out long offset))
            {
                Log.Error($"match failed");
                return Status.Failed;
            }

            for (int i = 0; i < count; i++)
            {
                //1. 创建Unit
                b2Body bodyA = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
                Unit unit = BulletManager.Instance.AddChild<Unit, int>(1001);
            
                //2. 添加组件
                GameObject bullet = GameObjectPoolHelper.GetObjectFromPool("GlinSpike");
                unit.AddComponent<GameObjectComponent>().GameObject = bullet;
                unit.AddComponent<BBParser>();

                //3. 设置偏移量
                int flip = i % 2 == 0 ? 1 : -1;
                int cnt = i / 2 + 1;
                b2Body bodyB = b2WorldManager.Instance.GetBody(unit.InstanceId);
                bodyB.SetPosition(bodyA.GetPosition() + new System.Numerics.Vector2( flip * cnt * offset / 10000f, -2f));
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}