using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableTargetComboCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableTargetComboCancel";
        }

        //EnableTargetCancel: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableTargetComboCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }
            
            parser.RemoveComponent<TargetComboCancelComponent>();
            if (!match.Groups["Enable"].Value.Equals("true")) return Status.Success;
           
            // 启动取消窗口
            parser.AddComponent<TargetComboCancelComponent>();
            
            // 启动 连段offset机制
            BuffManager buffManager = parser.GetParent<Unit>().GetComponent<BuffManager>();
            if (buffManager.GetComponent<ComboOffsetAbility>() == null)
            {
                buffManager.AddComponent<ComboOffsetAbility>(true);
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}