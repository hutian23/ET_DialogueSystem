using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableWhiffCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableWhiffCancel";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableWhiffCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }
            
            //1. 初始化
            parser.RemoveComponent<WhiffCancelComponent>();
            
            //2. 启动取消窗口
            if (match.Groups["Enable"].Value.Equals("true"))
            {
                parser.AddComponent<WhiffCancelComponent>();
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}