using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_FlipChange_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "FlipChange";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"FlipChange: (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            Unit unit = parser.GetParent<Unit>();
            InputComponent inputComponent = unit.GetComponent<InputComponent>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            FlipState preFlip = (FlipState)body.GetFlip();
            FlipState curFlip = preFlip;
            
            if (inputComponent.IsPressing(BBOperaType.LEFT) ||
                inputComponent.IsPressing(BBOperaType.DOWNLEFT) ||
                inputComponent.IsPressing(BBOperaType.UPLEFT))
            {
                curFlip = FlipState.Left;
            }
            else if (inputComponent.IsPressing(BBOperaType.RIGHT) ||
                     inputComponent.IsPressing(BBOperaType.DOWNRIGHT) ||
                     inputComponent.IsPressing(BBOperaType.UPRIGHT))
            {
                curFlip = FlipState.Right;
            }

            switch (match.Groups["Active"].Value)
            {
                case "true":
                    return curFlip != preFlip;
                case "false":
                    return curFlip == preFlip;
                default:
                    Log.Error("does not match FlipChange!");
                    return false;
            }
        }
    }
}