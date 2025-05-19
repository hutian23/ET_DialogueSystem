using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_PatrolReached_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "PatrolReached";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"PatrolReached: (?<Reached>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            PatrolComponent patrol = parser.GetParent<Unit>().GetComponent<PatrolComponent>();
            switch (match.Groups["Reached"].Value)
            {
                case "true":
                    return patrol.HasReached();
                case "false":
                    return !patrol.HasReached();
                default:
                    Log.Error($"matched failed");
                    return false;
            }
        }
    }
}