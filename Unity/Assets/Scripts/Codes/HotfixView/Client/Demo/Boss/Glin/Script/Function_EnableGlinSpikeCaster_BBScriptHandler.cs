using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableGlinSpikeCaster_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGlinSpikeCaster";
        }

        //EnableGlinSpikeCaster: SpawnCount, WaitFrame;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableGlinSpikeCaster: (?<SpawnCount>.*?), (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["SpawnCount"].Value, out int spawnCount) ||
                !int.TryParse(match.Groups["WaitFrame"].Value, out int waitFrame))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            parser.RemoveComponent<GlinSpikeCaster>();
            parser.AddComponent<GlinSpikeCaster>().StartSpawnCor(spawnCount, waitFrame);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}