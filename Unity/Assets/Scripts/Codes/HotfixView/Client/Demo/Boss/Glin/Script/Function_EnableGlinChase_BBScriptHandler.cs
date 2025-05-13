using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableGlinChase_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGlinChase";
        }
        
        //EnableGlinChase: active, minRotate, maxRotate, damping;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableGlinChase: (?<Active>\w+), (?<minRotate>.*?), (?<maxRotate>.*?), (?<damping>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["minRotate"].Value, out long minRotate) ||
                !long.TryParse(match.Groups["maxRotate"].Value, out long maxRotate) ||
                !long.TryParse(match.Groups["damping"].Value, out long damping))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            //1. 移除组件
            parser.RemoveComponent<GlinChaseComponent>();
            if(!match.Groups["Active"].Value.Equals("true")) return Status.Success;
            
            //2. 组件初始化
            GlinChaseComponent chase = parser.AddComponent<GlinChaseComponent>();
            chase.RotateCor(minRotate / 10000f, maxRotate / 10000f, damping / 10000f).Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}