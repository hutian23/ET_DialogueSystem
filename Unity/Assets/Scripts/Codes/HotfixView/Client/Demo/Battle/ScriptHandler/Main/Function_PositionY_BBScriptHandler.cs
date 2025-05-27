using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_PositionY_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "PositionY";
        }

        // PositionY: pos.Y;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "PositionY: (?<posY>.*?);");
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
            
            b2Body b2Body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            Vector2 pos = new(b2Body.GetPosition().X, posY / 10000f);
            b2Body.SetPosition(pos);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}