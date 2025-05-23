using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_BulletFaceToPlayer_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BulletFaceToPlayer";
        }

        //BulletFaceToPlayer: minAngle, maxAngle, Velocity;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "BulletFaceToPlayer: (?<minAngle>.*?), (?<maxAngle>.*?), (?<velocity>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["minAngle"].Value, out long minAngle) ||
                !long.TryParse(match.Groups["maxAngle"].Value, out long maxAngle) ||
                !long.TryParse(match.Groups["velocity"].Value, out long velocity))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            //1. 查询组件
            Unit player = BBUnitHelper.GetPlayer(parser.ClientScene());
            Unit unitB = parser.GetParent<Unit>();
            Unit bullet = Root.Instance.Get(parser.GetParam<long>("CreateBullet_UnitId")) as Unit;
            b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(unitB.InstanceId);
            b2Body bodyC = b2WorldManager.Instance.GetBody(bullet.InstanceId);

            //2. 射出方向
            Vector2 direction = (bodyB.GetPosition() - bodyA.GetPosition()).ToUnityVector2().normalized;
            float angle = Vector2.Angle(direction, Vector2.right);
            angle = Mathf.Clamp(angle, minAngle / 10000f, maxAngle / 10000f);
            
            bodyC.SetAngle(bodyB.GetFlip() * angle);
            
            //3. 射出速度
            Vector2 bulletVel = velocity * new Vector2(Mathf.Cos(bodyC.GetRadian()), Mathf.Sin(bodyC.GetRadian())) * new Vector2(1, -bodyB.GetFlip())/ 10000f;
            bodyC.SetVelocity(bulletVel.ToVector2());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}