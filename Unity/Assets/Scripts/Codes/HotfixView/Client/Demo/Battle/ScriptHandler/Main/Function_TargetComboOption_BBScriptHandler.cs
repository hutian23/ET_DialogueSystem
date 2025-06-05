using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_TargetComboOption_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TargetComboOption";
        }

        //TargetComboOption: BehaviorName, BuffFrame;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"TargetComboOption: (?<Option>\w+), (?<BuffFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }

            if (!int.TryParse(match.Groups["BuffFrame"].Value, out int buffFrame))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            // TargetCancel窗口中，将offsetBuffer按添加时间顺序依次取出，判断条件
            BuffManager buffManager = parser.GetParent<Unit>().GetComponent<BuffManager>();
            ComboOffsetAbility ability = buffManager.GetComponent<ComboOffsetAbility>();
            ability.BuffComboOffset(match.Groups["Option"].Value, buffFrame);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}