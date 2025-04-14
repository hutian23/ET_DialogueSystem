using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckRandom_BBScriptHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Random";
        }

        //Random: ran == 0
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Random: (?<Random>\w+) (?<Sign>[><=]+) (?<Value>\d+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            if (!int.TryParse(match.Groups["Value"].Value, out int checkVal))
            {
                Log.Error($"cannot format {match.Groups["Value"].Value} to int!!");
                return false;
            }

            string paramName = $"Random_{match.Groups["Random"].Value}";
            if (!parser.ContainParam(paramName))
            {
                Log.Error($"does not exist param: {paramName}");
                return false;
            }
            
            int value = parser.GetParam<int>(paramName);
            switch (match.Groups["Sign"].Value)
            {
                case "==":
                    return checkVal == value;
                case "<":
                    return value < checkVal;
                case ">":
                    return value > checkVal;
                case "<=":
                    return value <= checkVal;
                case ">=":
                    return value >= checkVal;
                default:
                    return false;
            }
        }
    }
}