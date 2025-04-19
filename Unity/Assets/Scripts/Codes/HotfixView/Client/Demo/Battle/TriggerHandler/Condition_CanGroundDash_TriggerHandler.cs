using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(GroundDashAbility))]
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
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            GroundDashAbility gd = buffManager.GetComponent<GroundDashAbility>();

            if (gd == null)
            {
                Log.Error($"does not exist GroundDashAbility!!!");
                return false;
            }

            switch (match.Groups["CanDash"].Value)
            {
                case "true":
                    return gd.dashCount > 0;
                case "false":
                    return gd.dashCount <= 0;
                default:
                    return false;
            }
        }
    }
}