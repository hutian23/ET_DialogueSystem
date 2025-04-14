using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckNumeric_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Numeric";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine,@"Numeric: (?<NumericType>\w+) (?<Sign>[><=]+) (?<Value>\d+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            Unit unit = parser.GetParent<Unit>();
            BBNumeric numeric = unit.GetComponent<BBNumeric>();

            long value = numeric.GetAsLong(match.Groups["NumericType"].Value);
            if (!long.TryParse(match.Groups["Value"].Value, out long checkValue))
            {
                Log.Error($"cannot convert {match.Groups["CheckValue"].Value} to long");
                return false;
            }
            
            switch (match.Groups["Sign"].Value)
            {
                case "<":
                    return value < checkValue;
                case "==":
                    return value == checkValue;
                case ">":
                    return value > checkValue;
                case ">=":
                    return value >= checkValue;
                case "<=":
                    return value <= checkValue;
                default:
                    return false;
            }
        }
    }
}