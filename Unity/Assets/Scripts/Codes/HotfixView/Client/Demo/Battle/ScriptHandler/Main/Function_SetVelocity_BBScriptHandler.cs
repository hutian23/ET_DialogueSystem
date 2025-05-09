using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_SetVelocity_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetVelocity";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SetVelocity: (?<VelocityX>.*?), (?<VelocityY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["VelocityX"].Value, out long velocityX) || 
                !long.TryParse(match.Groups["VelocityY"].Value, out long velocityY))
            {
                Log.Error($"cannot format {match.Groups["Velocity"].Value} to long");
                return Status.Failed;
            }

            b2Body body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            body.SetVelocity(new Vector2(velocityX, velocityY) / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}