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
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }

            InputWait wait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();
            return wait.CheckInput(match.Groups["InputType"].Value);
        }
    }
}