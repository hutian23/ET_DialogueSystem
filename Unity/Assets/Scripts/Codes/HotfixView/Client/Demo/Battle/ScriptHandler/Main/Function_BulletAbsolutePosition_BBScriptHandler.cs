using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_BulletAbsolutePosition_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BulletAbsolutePosition";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BulletAbsolutePosition: (?<PosX>.*?), (?<PosY>.*?);");
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
            Unit unitB = Root.Instance.Get(instanceId) as Unit;
            b2Body bodyB = b2WorldManager.Instance.GetBody(unitB.InstanceId);
            
            //2. 设置绝对位置
            bodyB.SetPosition(new Vector2(posX, posY) / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}