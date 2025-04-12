using System.Text.RegularExpressions;

namespace ET.Client
{
    public class EnableTargetCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableTargetCancel";
        }

        //EnableTargetCancel: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableTargetCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }
            
            //1. 初始化
            parser.RemoveComponent<TargetCancelComponent>();
            
            //2. 启动取消窗口
            if (match.Groups["Enable"].Value.Equals("true"))
            {
                parser.AddComponent<TargetCancelComponent>();
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}