using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_TransitionCached_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "TransitionCached";
        }

        //TransitionCached: NoSquat;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"TransitionCached: (?<transition>\w+), (?<enable>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            Unit unit = parser.GetParent<Unit>();
            Transition transition = unit.GetComponent<Transition>();
            
            string transitionFlag = match.Groups["transition"].Value;
            return match.Groups["enable"].Value.Equals("true")? transition.CheckCachedFlag(transitionFlag) : !transition.CheckCachedFlag(transitionFlag);
        }
    }
}