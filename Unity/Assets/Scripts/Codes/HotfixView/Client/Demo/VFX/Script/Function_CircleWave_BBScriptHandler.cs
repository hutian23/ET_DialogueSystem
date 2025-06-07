using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_CircleWave_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CircleWave";
        }

        // CircleWaveEffect: waveWidth, waveSpeed, totalTick;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CircleWave: (?<waveWidth>.*?), (?<waveSpeed>.*?), (?<totalTick>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["waveWidth"].Value, out long waveWidth) ||
                !long.TryParse(match.Groups["waveSpeed"].Value, out long waveSpeed) ||
                !int.TryParse(match.Groups["totalTick"].Value, out int totalTick))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            parser.RemoveComponent<CircleWaveComponent>();
            parser.AddComponent<CircleWaveComponent, float, float, int>(waveWidth / 10000f, waveSpeed / 10000f, totalTick, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}