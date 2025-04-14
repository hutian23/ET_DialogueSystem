using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RootInit_PoolObject_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "PoolObject";
        }

        //PoolObject: 'SlashRing', 2;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"PoolObject: '(?<Object>.*?)', (?<Count>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["Count"].Value, out var count))
            {
                Log.Error($"cannot format {match.Groups["Count"].Value} to int!!!");
                return Status.Failed;
            }

            GameObjectPoolHelper.InitPool(match.Groups["Object"].Value, count);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}