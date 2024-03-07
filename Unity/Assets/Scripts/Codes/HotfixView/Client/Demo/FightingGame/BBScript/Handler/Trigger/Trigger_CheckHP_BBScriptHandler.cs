using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Trigger_CheckHP_BBScriptHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "HP";
        }

        //if HP > 10:
        public override bool Check(BBParser parser, BBScriptData data)
        {
            NumericComponent nu = parser.GetParent<DialogueComponent>().GetParent<Unit>().GetComponent<NumericComponent>();
            Log.Warning(nu[NumericType.Hp].ToString());

            Match match = Regex.Match(data.opLine, "HP (?<OP>.*?) (?<Numeric>.*?)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }

            int.TryParse(match.Groups["Numeric"].Value, out int checkValue);
            switch (match.Groups["OP"].Value)
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