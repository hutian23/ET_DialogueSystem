using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_BulletAngle_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BulletAngle";
        }

        //BulletAngle: 300000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "BulletAngle: (?<Angle>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["Angle"].Value, out long angle))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            long instanceId = parser.GetParam<long>("CreateBullet_UnitId");
            Unit unitB = Root.Instance.Get(instanceId) as Unit;
            b2Body bodyB = b2WorldManager.Instance.GetBody(unitB.InstanceId);
            
            bodyB.SetRotation((float)Box2DHelper.DegreesToRadians(angle / 10000f));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}