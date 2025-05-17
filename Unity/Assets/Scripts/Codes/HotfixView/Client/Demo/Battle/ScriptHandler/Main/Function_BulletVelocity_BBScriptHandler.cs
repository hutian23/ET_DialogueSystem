using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_BulletVelocity_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BulletVelocity";
        }

        //BulletVelocity: VelX, VelY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BulletVelocity: (?<VelX>.*?), (?<VelY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["VelX"].Value, out long velX) || !long.TryParse(match.Groups["VelY"].Value, out long velY))
            {
                Log.Error($"cannot format {match.Groups["VelX"].Value} / {match.Groups["VelY"].Value} to long!!!");
                return Status.Failed;
            }
            
            long instanceId = parser.GetParam<long>("CreateBullet_UnitId");
            Unit unitB = Root.Instance.Get(instanceId) as Unit;
            b2Body bodyB = b2WorldManager.Instance.GetBody(unitB.InstanceId);
            
            bodyB.SetVelocity(new Vector2(velX, velY) / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}