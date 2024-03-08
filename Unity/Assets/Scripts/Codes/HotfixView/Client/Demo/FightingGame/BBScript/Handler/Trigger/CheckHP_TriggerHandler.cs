using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckHP_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "HP";
        }

        //if HP > 10:
        public override bool Check(BBParser parser, BBScriptData data)
        {
            NumericComponent nu = parser.GetParent<DialogueComponent>().GetParent<Unit>().GetComponent<NumericComponent>();

            Match match = Regex.Match(data.opLine, @"(\w+)\s*([<>=]+)\s*(\d+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
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

            return false;
        }
    }
}