using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_AirDashPos_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AirDashPos";
        }

        //AirDashPos: PosY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AirDashPos: (?<posY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"matched Failed");
                return Status.Failed;
            }
            
            Unit player = BBUnitHelper.GetPlayer(parser.ClientScene());
            Unit unit = parser.GetParent<Unit>();
            b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(unit.InstanceId);

            float x = bodyA.GetPosition().X;
            float y = posY / 10000f;
            
            bodyB.SetPosition(new Vector2(x, y));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}