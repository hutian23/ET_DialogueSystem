using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_BulletLocalPosition_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BulletLocalPosition";
        }

        //BulletPos: 10000, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BulletLocalPosition: (?<PosX>.*?), (?<PosY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["PosX"].Value, out long posX) || !long.TryParse(match.Groups["PosY"].Value, out long posY))
            {
                Log.Error($"cannot format {match.Groups["PosX"].Value} / {match.Groups["PosY"].Value} to long!!!");
                return Status.Failed;
            }
            
            //1. 查询组件
            long instanceId = parser.GetParam<long>("CreateBullet_UnitId");
            Unit unitA = parser.GetParent<Unit>();
            Unit unitB = Root.Instance.Get(instanceId) as Unit;
            b2Body bodyA = b2WorldManager.Instance.GetBody(unitA.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(unitB.InstanceId);
            
            //2. 设置相对位置
            Vector2 pos = bodyA.GetPosition();
            Vector2 offSet = new Vector2(posX * bodyA.GetFlip(), posY) / 10000f;
            bodyB.SetPosition(pos + offSet);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}