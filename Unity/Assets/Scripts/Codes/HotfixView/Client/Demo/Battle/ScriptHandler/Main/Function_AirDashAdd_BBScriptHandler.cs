using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_AirDashAdd_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AirDashAdd";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"AirDashAdd: (?<Count>.*?);");
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

            //2. 设置当前冲刺次数
            Unit unit = parser.GetParent<Unit>();
            AirDashAbility ad = unit.GetComponent<BuffManager>().GetComponent<AirDashAbility>();
            if (ad == null)
            {
                Log.Error($"cannot found AirDashAbility !!!");
                return Status.Failed;
            }
            
            int curCount = ad.GetDashCount();
            ad.SetDashCount(curCount + count);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}