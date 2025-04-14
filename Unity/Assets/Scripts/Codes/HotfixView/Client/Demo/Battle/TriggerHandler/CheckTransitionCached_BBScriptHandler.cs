using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckTransitionCached_BBScriptHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "TransitionCached";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"TransitionCached: '(?<transition>\w+)', (?<exist>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            string transitionFlag = $"Transition_{match.Groups["transition"].Value}";

            switch (match.Groups["exist"].Value)
            {
                case "true":
                    return parser.ContainParam(transitionFlag);
                case "false":
                    return !parser.ContainParam(transitionFlag);
                default:
                    Log.Error($"cannot match flag exist state");
                    return false;
            }
        }
    }
}