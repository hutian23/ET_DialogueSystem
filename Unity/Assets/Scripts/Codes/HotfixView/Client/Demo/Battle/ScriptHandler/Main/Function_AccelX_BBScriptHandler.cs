using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_AccelX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AccelX";
        }
        
        //AccelX: StartX, LastFrame, AccelX;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "AccelX: (?<startV>.*?), (?<accel>.*?), (?<lastFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["startV"].Value, out long startV) ||
                !int.TryParse(match.Groups["lastFrame"].Value, out int lastFrame) ||
                !long.TryParse(match.Groups["accel"].Value, out long accel))
            {
                Log.Error($"match failed");
                return Status.Failed;
            }

            parser.RemoveComponent<AccelXComponent>();
            parser.AddComponent<AccelXComponent, float, float, int>(startV / 10000f, accel / 10000f, lastFrame, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}