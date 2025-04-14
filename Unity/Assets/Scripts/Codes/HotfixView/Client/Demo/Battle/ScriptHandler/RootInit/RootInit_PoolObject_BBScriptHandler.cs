using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RootInit_PoolObject_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "PoolObject";
        }

        //PoolObject: SlashRing, 2;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"PoolObject: (?<Object>\w+), (?<Count>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["Count"].Value, out int count))
            {
                Log.Error($"cannot format {match.Groups["Count"].Value} to int!!!");
                return Status.Failed;
            }

            //2. 对象池添加 PoolObject
            GameObjectPoolHelper.InitPool(match.Groups["Object"].Value, count);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}