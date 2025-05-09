using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_BulletPos_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BulletPos";
        }

        //BulletPos: 10000, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BulletPos: (?<PosX>.*?), (?<PosY>.*?);");
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
            
            b2Body bodyA = parser.GetParent<Unit>().GetComponent<b2Body>();
            
            long instanceId = parser.GetParam<long>("CreateBullet_UnitId");
            Unit unit = Root.Instance.Get(instanceId) as Unit;
            b2Body bodyB = unit.GetComponent<b2Body>();
            
            //1. Caster Position
            Vector2 pos = bodyA.GetPosition();
            Vector2 offSet = new Vector2(posX * bodyA.GetFlip(), posY) / 10000f;
            bodyB.SetPosition(pos + offSet);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}