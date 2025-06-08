using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_InAttackRange_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "InAttackRange";
        }

        // InAttackRange: Active;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"InAttackRange: (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            AttackRangeComponent attackRange = parser.GetComponent<AttackRangeComponent>();
            if (attackRange == null)
            {
                Log.Error($"does not exist attackRangeComponent, unit.InstanceId: {parser.GetParent<Unit>().InstanceId}");
                return false;
            }

            switch (match.Groups["Active"].Value)
            {
                case "true":
                    return attackRange.InAttackRange();
                case "false":
                    return !attackRange.InAttackRange();
                default:
                    Log.Error($"matched failed");
                    return false;
            }
        }
    }
}