using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckFlag_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Flag";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Flag: (?<Flag>\w+), (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            switch (match.Groups["Active"].Value)
            {
                case "true":
                    return parser.ContainParam($"Flag_{match.Groups["Flag"].Value}");
                case "false":
                    return !parser.ContainParam($"Flag_{match.Groups["Flag"].Value}");
                default:
                    return false;
            }
        }
    }
}