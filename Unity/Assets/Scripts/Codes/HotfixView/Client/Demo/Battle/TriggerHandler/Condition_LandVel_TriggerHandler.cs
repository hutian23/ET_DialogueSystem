using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_LandVel_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "LandVel";
        }

        //LandVel: 400000
        public override bool Check(BBParser parser, BBScriptData data)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"LandVel: (?<Velocity>-?\d+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            if (!long.TryParse(match.Groups["Velocity"].Value, out long landVel))
            {
                Log.Error($"cannot format {match.Groups["Velocity"].Value} to long!!!");
                return false;
            }
            
            //2. 
            Unit unit = parser.GetParent<Unit>();
            AirCheckAbility ability = unit.GetComponent<BuffManager>().GetComponent<AirCheckAbility>();

            float checkValue = landVel / 10000f;
            float curValue = Math.Abs(ability.GetLandVel().Y);

            return curValue >= checkValue;
        }
    }
}