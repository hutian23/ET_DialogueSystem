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
            long instanceId = parser.GetParam<long>("CreateBullet_UnitId");
            Unit player = BBUnitHelper.GetPlayer(parser.ClientScene());
            Unit unitB = Root.Instance.Get(instanceId) as Unit;
            b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(unitB.InstanceId);

            //2. 射出方向
            // Vector2 direction = (bodyB.GetPosition() - bodyA.GetPosition()).ToUnityVector2().normalized;
            // float angle = Vector2.Angle(direction, Vector2.right);
            Log.Warning((bodyA.GetFlip() * minAngle / 10000f).ToString());
            
            bodyB.SetRotation(bodyA.GetFlip() * minAngle / 10000f * Mathf.Deg2Rad);
            
            //3. 射出速度
            // bodyB.SetVelocity(velocity / 10000f * new System.Numerics.Vector2(1f, 1f));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}