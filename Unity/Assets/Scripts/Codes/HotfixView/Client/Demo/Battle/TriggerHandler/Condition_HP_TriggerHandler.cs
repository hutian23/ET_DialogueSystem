using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_HP_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "HP";
        }

        // HP: Value > 0
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"HP: Value (?<Sign>[><=]+) (?<CheckVel>-?\d+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            if (!int.TryParse(match.Groups["CheckVel"].Value, out int checkVel))
            {
                Log.Error($"cannot format {match.Groups["Position"].Value} to int !!!");
                return false;
            }

            HPAbility ability = parser.GetParent<Unit>().GetComponent<BuffManager>().GetComponent<HPAbility>();
            if (ability == null)
            {
                Log.Error($"does not exist component: HPAbility. unit.instanceId: {parser.GetParent<Unit>().InstanceId}");
                return false;
            }

            int targetVel = ability.GetHP();
            switch (match.Groups["Sign"].Value)
            {
                case "=":
                    return targetVel == checkVel;
                case ">":
                    return targetVel > checkVel;
                case "<":
                    return targetVel < checkVel;
                case ">=":
                    return targetVel >= checkVel;
                case "<=":
                    return targetVel <= checkVel;
                default:
                    return false;
            }
        }
    }
}