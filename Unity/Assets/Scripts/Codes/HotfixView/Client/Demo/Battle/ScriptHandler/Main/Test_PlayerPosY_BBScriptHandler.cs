using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Test_PlayerPosY_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test_SetPlayerPosY";
        }

        //Test_PlayerPosY: 2000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Test_SetPlayerPosY: (?<Position>\d+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["Position"].Value, out long posY))
            {
                Log.Error($"cannot format {match.Groups["Position"].Value} to long!!!");
                return Status.Failed;
            }

            Unit player = TODUnitHelper.GetPlayer(parser.ClientScene());
            b2Body playerBody = b2WorldManager.Instance.GetBody(player.InstanceId);
            b2Body body = b2WorldManager.Instance.GetBody(parser.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            
            Vector2 targetPos = new(playerBody.GetPosition().X, posY / 1000f);
            body.SetPosition(targetPos);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}