using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_CanJustEvade_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "CanJustEvade";
        }

        // CanJustEvade: false;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"CanJustEvade: (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            JustEvadeAbility ability = parser.GetParent<Unit>().GetComponent<BuffManager>().GetComponent<JustEvadeAbility>();
            switch (match.Groups["Active"].Value)
            {
                case "true":
                    return ability.CanJustEvade();
                case "false":
                    return !ability.CanJustEvade();
                default:
                    Log.Error($"matched failed");
                    return false;
            }
        }
    }
}