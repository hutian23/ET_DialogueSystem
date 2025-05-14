using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_InRange_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "InRange";
        }

        // InRange: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"InRange: (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            InRangeCheckComponent component = parser.GetComponent<InRangeCheckComponent>();

            switch (match.Groups["Active"].Value)
            {
                case "true":
                    return component.InRange();
                case "false":
                    return !component.InRange();
                default:
                    Log.Error($"matched failed");
                    return false;
            }
        }
    }
}