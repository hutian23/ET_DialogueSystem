using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableCastGlinFireball_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableCastGlinFireball";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableCastGlinFireball: (?<lastFrame>.*?), (?<waitFrame>.*?), (?<startV>.*?), (?<accelX>.*?), (?<accelY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["lastFrame"].Value, out int lastFrame) ||
                !int.TryParse(match.Groups["waitFrame"].Value, out int waitFrame) || 
                !long.TryParse(match.Groups["startV"].Value, out long startV) ||
                !long.TryParse(match.Groups["accelX"].Value, out long accelX) ||
                !long.TryParse(match.Groups["accelY"].Value, out long accelY))
            {
                Log.Error("matched failed");
                return Status.Failed;
            }
            
            parser.RemoveComponent<GlinFireBallCaster>();
            GlinFireBallCaster caster = parser.AddComponent<GlinFireBallCaster>();
            caster.StartSpawnCor(lastFrame, waitFrame, startV / 10000f, accelX / 10000f, accelY / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}