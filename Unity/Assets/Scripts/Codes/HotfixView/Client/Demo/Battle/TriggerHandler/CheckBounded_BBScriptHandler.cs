using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckBounded_BBScriptHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Bounded";
        }

        // Bounded: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Bounded: (?<Flag>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            if (!parser.ContainParam("BounceCheck_Flag"))
            {
                return false;
            }
            
            switch (match.Groups["Flag"].Value)
            {
                case "true":
                    return parser.GetParam<bool>("BounceCheck_Flag");
                case "false":
                    return !parser.GetParam<bool>("BounceCheck_Flag");
                default:
                    return false;
            }
        }
    }
}