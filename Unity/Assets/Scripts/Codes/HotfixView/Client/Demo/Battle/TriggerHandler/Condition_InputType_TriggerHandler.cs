using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_InputType_TriggerHandler: BBTriggerHandler
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
            InputComponent inputComponent = unit.GetComponent<InputComponent>();
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            
            return inputComponent.CheckBuffer(match.Groups["InputType"].Value, sceneTimer.GetNow());
        }
    }
}