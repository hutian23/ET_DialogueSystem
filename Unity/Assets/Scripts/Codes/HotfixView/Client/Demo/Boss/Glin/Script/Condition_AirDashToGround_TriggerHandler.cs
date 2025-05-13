using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_AirDashToGround_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "AirDashToGround";
        }

        //AirDashToGround: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"AirDashToGround: (?<AirDashToGround>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            AirDashToGroundComponent airDash = parser.GetComponent<AirDashToGroundComponent>();
            
            switch (match.Groups["AirDashToGround"].Value)
            {
                case "true":
                    return airDash.GetOnGround();
                case "false":
                    return !airDash.GetOnGround();
                default:
                    Log.Error($"matched failed");
                    return false;
            }
        }
    }
}