using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_Transition_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Transition";
        }

        //Transition: 'RunToIdle', true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Transition: '(?<transition>\w+)', (?<exist>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            string transitionFlag = $"Transition_{match.Groups["transition"].Value}";

            switch (match.Groups["exist"].Value)
            {
                case "true":
                    return parser.GetParent<Unit>().GetComponent<BehaviorMachine>().ContainTmpParam(transitionFlag);
                case "false":
                    return !parser.GetParent<Unit>().GetComponent<BehaviorMachine>().ContainTmpParam(transitionFlag);
                default:
                    Log.Error($"cannot match flag exist state");
                    return false;
            }
        }
    }
}