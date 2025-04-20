using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_CanAirDash_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "CanAirDash";
        }

        //CanAirDash: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"CanAirDash: (?<CanDash>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            Unit unit = parser.GetParent<Unit>();
            AirDashAbility ad = unit.GetComponent<BuffManager>().GetComponent<AirDashAbility>();

            //1. 未查询到能力组件，认为不能进行地面冲刺
            if (ad == null) return false;

            //2. 地面冲刺次数不为0
            return match.Groups["CanDash"].Value.Equals("true") ? ad.GetDashCount() > 0 : ad.GetDashCount() <= 0;
        }
    }
}