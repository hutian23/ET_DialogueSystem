using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckCounter_BBScriptHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Counter";
        }

        //Counter: Value > 10
        public override bool Check(BBParser parser, BBScriptData data)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"Counter: Value (?<Sign>[><=]+) (?<CheckVel>-?\d+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            if (!int.TryParse(match.Groups["CheckVel"].Value, out int checkVel))
            {
                Log.Error($"cannot format {match.Groups["Position"].Value} to int !!!");
                return false;
            }

            //2. 查询组件
            Counter counter = parser.GetComponent<Counter>();
            if (counter == null)
            {
                Log.Error($"does not exist component: Counter");
                return false;
            }
            
            int targetVel = counter.GetCounter();
            switch (match.Groups["Sign"].Value)
            {
                case "=":
                    return targetVel == checkVel;
                case ">":
                    return targetVel > checkVel;
                case "<":
                    return targetVel < checkVel;
                case ">=":
                    return targetVel >= checkVel;
                case "<=":
                    return targetVel <= checkVel;
                default:
                    return false;
            }
        }
    }
}