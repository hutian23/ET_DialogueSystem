using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckVelocity_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Velocity";
        }

        //Velocity: Y <= 10
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Velocity: (?<XY>\w+) (?<Sign>[><=]+) (?<Velocity>-?\d+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            if (!long.TryParse(match.Groups["Velocity"].Value, out long Velocity))
            {
                Log.Error($"cannot format {match.Groups["Position"].Value} to long !!!");
                return false;
            }
            
            Unit unit = parser.GetParent<Unit>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();

            float _targetValue = match.Groups["XY"].Value.Equals("X")? b2Unit.GetVelocity().X : b2Unit.GetVelocity().Y;
            long targetVel = (long)(_targetValue * 10000);
            
            switch (match.Groups["Sign"].Value)
            {
                case "=":
                    return targetVel == Velocity;
                case ">":
                    return targetVel > Velocity;
                case "<":
                    return targetVel < Velocity;
                case ">=":
                    return targetVel >= Velocity;
                case "<=":
                    return targetVel <= Velocity;
                default:
                    return false;
            }
        }
    }
}