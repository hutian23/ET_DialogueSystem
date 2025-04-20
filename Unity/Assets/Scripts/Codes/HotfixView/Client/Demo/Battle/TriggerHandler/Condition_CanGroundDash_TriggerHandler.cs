using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_CanGroundDash_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "CanGroundDash";
        }

        //CanGroundDash: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"CanGroundDash: (?<CanDash>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            Unit unit = parser.GetParent<Unit>();
            GroundDashAbility gd = unit.GetComponent<BuffManager>().GetComponent<GroundDashAbility>();

            //1. 未查询到能力组件，认为不能进行地面冲刺
            if (gd == null) return false;

            //2. 地面冲刺次数不为0
            return match.Groups["CanDash"].Value.Equals("true") ? gd.GetDashCount() > 0 : gd.GetDashCount() <= 0;
        }
    }
}