using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckLandVelocity_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "LandVelocity";
        }

        //LandVelocity: -400000;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"LandVelocity: (?<Vel>\d+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            if (!long.TryParse(match.Groups["Vel"].Value, out long landVel))
            {
                Log.Error($"cannot format {match.Groups["Vel"].Value} to long!!!");
                return false;
            }
            
            float checkValue = landVel / 10000f;
            BehaviorMachine machine = parser.GetParent<Unit>().GetComponent<BehaviorMachine>();
            
            return machine.ContainParam("LandVelocity") && machine.GetParam<float>("LandVelocity") <= -checkValue;
        }
    }
}