using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(GroundDashAbility))]
    public class Function_EnableGroundDash_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGroundDash";
        }

        //EnableGroundDash: 地面最大冲刺次数, 间隔多少帧完成充能
        //EnableGroundDash: 2, 40;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"EnableGroundDash: (?<MaxDash>.*?), (?<ChargeFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["MaxDash"].Value, out int maxDash) ||
                !int.TryParse(match.Groups["ChargeFrame"].Value, out int chargeFrame))
            {
                Log.Error($"cannot format {match.Groups["MaxDash"].Value} / {match.Groups["ChargeFrame"].Value} to int!!");
                return Status.Failed;
            }
            
            //2. 添加组件，表示Unit可以进行冲刺
            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            GroundDashAbility ability = buffManager.AddComponent<GroundDashAbility>();
            
            //3. 数值初始化
            ability.dashCount = maxDash;
            ability.maxDashCount = maxDash;
            ability.chargeFrame = chargeFrame;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}