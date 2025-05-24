using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_SpawnEnemy_Position_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SpawnEnemy_Position";
        }

        // EnemyPosition: posX, posY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SpawnEnemy_Position: (?<posX>.*?), (?<posY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["posX"].Value, out long posX) || !long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            long instanceId = parser.GetParam<long>("SpawnEnemy_InstanceId");
            Unit enemy = Root.Instance.Get(instanceId) as Unit;
            b2Body body = b2WorldManager.Instance.GetBody(enemy.InstanceId);
            body.SetPosition(new Vector2(posX, posY) / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}