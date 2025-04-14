using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckPosition_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Position";
        }
        
        //Position: Y >= 10000
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Position: (?<XY>\w+) (?<Sign>[><=]+) (?<Position>\d+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            if (!long.TryParse(match.Groups["Position"].Value, out long Position))
            {
                Log.Error($"cannot format {match.Groups["Position"].Value} to long !!!");
                return false;
            }
            
            Unit unit = parser.GetParent<Unit>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            float _targetValue = match.Groups["XY"].Value.Equals("X")? b2Body.GetPosition().X : b2Body.GetPosition().Y;
            long targetPosition = (long)(_targetValue * 10000);
            
            switch (match.Groups["Sign"].Value)
            {
                case "=":
                    return targetPosition == Position;
                case ">":
                    return targetPosition > Position;
                case "<":
                    return targetPosition < Position;
                case ">=":
                    return targetPosition >= Position;
                case "<=":
                    return targetPosition <= Position;
                default:
                    return false;
            }
        }
    }
}