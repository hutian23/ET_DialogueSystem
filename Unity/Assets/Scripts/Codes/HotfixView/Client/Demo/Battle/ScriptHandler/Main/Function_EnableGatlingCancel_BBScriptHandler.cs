using System.Text.RegularExpressions;

namespace ET.Client
{
    // 加特林取消，当前动作只能被比自己层级高 or 添加了取消标签的动作取消
    public class Function_EnableGatlingCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGatlingCancel";
        }

        //EnableGatlingCancel: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableGatlingCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }
            
            //1. 初始化
            parser.RemoveComponent<GatlingCancelComponent>();
            
            //2. 启动取消窗口
            if (match.Groups["Enable"].Value.Equals("true"))
            {
                parser.AddComponent<GatlingCancelComponent>();
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}