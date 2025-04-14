using System.Text.RegularExpressions;

namespace ET.Client
{
    public class ThrowHurt_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "ThrowHurt";
        }

        //ThrowHurt: false;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"ThrowHurt: (?<Result>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            switch (match.Groups["Result"].Value)
            {
                case "true":
                    return parser.ContainParam("TargetBind_ThrowHurt");
                case "false":
                    return !parser.ContainParam("TargetBind_ThrowHurt");
                default:
                    Log.Error($"does not match throw hurt result");
                    return false;
            }
        }
    }
}