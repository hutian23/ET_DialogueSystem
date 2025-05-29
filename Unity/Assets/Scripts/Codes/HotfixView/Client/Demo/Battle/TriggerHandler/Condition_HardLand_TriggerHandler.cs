using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_HardLand_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "HardLand";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"HardLand: (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            //2. 查询组件
            HardLandCheckComponent hardLand = parser.GetComponent<HardLandCheckComponent>();
            
            switch (match.Groups["Active"].Value)
            {
                case "true":
                    return hardLand.GetHardLand();
                case "false":
                    return !hardLand.GetHardLand();
                default:
                    Log.Error("does not match inAir!");
                    return false;
            }
        }
    }
}