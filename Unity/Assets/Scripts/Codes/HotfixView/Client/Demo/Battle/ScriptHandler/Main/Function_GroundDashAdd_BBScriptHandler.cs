using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_GroundDashAdd_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GroundDashAdd";
        }

        //GroundDashAdd: -1; 
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"GroundDashAdd: (?<Count>.*?);");
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
            GroundDashAbility gd = unit.GetComponent<BuffManager>().GetComponent<GroundDashAbility>();
            if (gd == null)
            {
                Log.Error($"cannot found GroundDashAbility !!!");
                return Status.Failed;
            }

            int curCount = gd.Get();
            gd.Set(curCount + count);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}