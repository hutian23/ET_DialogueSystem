using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_Flip_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Flip";
        }
        
        //Flip: Left
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine,@"Flip: (?<Flip>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            b2Body body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            FlipState checkFlip = match.Groups["Flip"].Value.Equals("Left")? FlipState.Left : FlipState.Right;
            return body.GetFlip() == (int)checkFlip;
        }
    }
}