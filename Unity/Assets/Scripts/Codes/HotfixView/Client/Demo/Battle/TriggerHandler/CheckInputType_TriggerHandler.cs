using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckInputType_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "InputType";
        }

        //InputType: (RunHold);
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"InputType: ((?<InputType>\w+))");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            Unit unit = parser.GetParent<Unit>();
            InputWait inputWait = unit.GetComponent<InputWait>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            
            return inputWait.CheckBuffer(match.Groups["InputType"].Value, bbTimer.GetNow());
        }
    }
}