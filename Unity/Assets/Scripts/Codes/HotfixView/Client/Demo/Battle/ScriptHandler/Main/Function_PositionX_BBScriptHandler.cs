using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_PositionX_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "PositionX";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "PositionX: (?<posX>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["posX"].Value, out long posX))
            {
                Log.Error($"cannot format posX to long");
                return Status.Failed;
            }
            
            b2Body b2Body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            Vector2 pos = new(posX / 10000f, b2Body.GetPosition().Y);
            b2Body.SetPosition(pos);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}