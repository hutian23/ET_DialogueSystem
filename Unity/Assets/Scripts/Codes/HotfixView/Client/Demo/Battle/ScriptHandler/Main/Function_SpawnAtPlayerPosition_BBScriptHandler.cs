using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_SpawnAtPlayerPosition_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SpawnAtPlayerPosition";
        }

        // SpawnAtPlayerPosition: posY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "SpawnAtPlayerPosition: (?<posY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"cannot format posY to long");
                return Status.Failed;
            }
            
            Unit player = BBUnitHelper.GetPlayer();
            Unit unit = parser.GetParent<Unit>();
            b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(unit.InstanceId);
            
            bodyB.SetPosition(new Vector2(bodyA.GetPosition().X, posY / 10000f));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}