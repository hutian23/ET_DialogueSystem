using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_CastGlinSpike_Step2_BBScriptHandler_Obselete: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CastGlinSpike_Step2";
        }

        //CastGlinSpike_Step2: randX, posY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CastGlinSpike_Step2: (?<posY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            //1. 创建 bullet unit
            Unit player = BBUnitHelper.GetPlayer();
            Unit bullet = BulletManager.Instance.AddChild<Unit, int>(1001);
            UnityEngine.GameObject go = GameObjectPoolHelper.GetObjectFromPool("GlinSpike_Step2");
            bullet.AddComponent<GameObjectComponent>().GameObject = go;
            bullet.AddComponent<BBParser>();

            //2.    
            b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(bullet.InstanceId);

            float x = bodyA.GetPosition().X;
            float y = posY / 10000f;
            bodyB.SetPosition(new Vector2(x, y));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}