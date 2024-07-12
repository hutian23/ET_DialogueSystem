using System.Text.RegularExpressions;

namespace ET.Client.Trigger
{
    public class CheckHP_TriggerHandler: TriggerHandler
    {
        public override string GetTriggerType()
        {
            return "HP";
        }

        //HP > 10
        public override bool Check(ScriptParser parser, ScriptData data)
        {
            NumericComponent nu = parser.GetUnit().GetComponent<NumericComponent>();

            Match match = Regex.Match(data.opLine, @"(\w+)\s*([<>=]+)\s*(\d+)");
            if (!match.Success)
            {
                ScriptHelper.ScriptMatchError(data.opLine);
                return false;
            }

            int.TryParse(match.Groups[3].Value, out int checkValue);
            switch (match.Groups[2].Value)
            {
                case "<":
                    return nu[NumericType.Hp] < checkValue;
                case "=":
                    return nu[NumericType.Hp] == checkValue;
                case ">":
                    return nu[NumericType.Hp] > checkValue;
            }

            return true;
        }
    }
}