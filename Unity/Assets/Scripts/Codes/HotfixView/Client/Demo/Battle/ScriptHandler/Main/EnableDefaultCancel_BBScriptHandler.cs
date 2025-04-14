using System.Text.RegularExpressions;

namespace ET.Client
{
    public class EnableDefaultCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableDefaultCancel";
        }

        //处于中立状态，可以切换进权值比自己高的动作
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableDefaultCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }
            
            //1. 初始化
            parser.RemoveComponent<DefaultCancelComponent>();
            
            //2. 启动取消窗口
            if (match.Groups["Enable"].Value.Equals("true"))
            {
                parser.AddComponent<DefaultCancelComponent>();
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}