using System.Text.RegularExpressions;

namespace ET.Client
{
    //EnableNandemoCancel: true;
    public class EnableNandemoCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableNandemoCancel";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableNandemoCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //1. 初始化
            parser.RemoveComponent<NandemoCancelComponent>();
           
            //2. 启动取消窗口
            if (match.Groups["Enable"].Value.Equals("true"))
            {
                parser.AddComponent<NandemoCancelComponent>();   
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}