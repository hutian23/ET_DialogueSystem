using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_Transition_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Transition";
        }

        //Transition: RunToIdle, true
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Transition: (?<transition>\w+), (?<enable>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            Unit unit = parser.GetParent<Unit>();
            Transition transition = unit.GetComponent<Transition>();
            
            string transitionFlag = match.Groups["transition"].Value;
            return match.Groups["enable"].Value.Equals("true")? transition.CheckFlag(transitionFlag) : !transition.CheckFlag(transitionFlag);
        }
    }
}