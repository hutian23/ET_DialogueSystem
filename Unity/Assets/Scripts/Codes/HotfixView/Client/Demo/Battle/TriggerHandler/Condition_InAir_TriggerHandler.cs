using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_InAir_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "InAir";
        }

        //inAir: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"InAir: (?<InAir>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            //2. 查询组件
            AirCheckComponent airCheck = parser.GetParent<Unit>().GetBuff<AirCheckComponent>();
            if (airCheck == null)
            {
                Log.Error($"does not exist AirCheckComponent!");
                return false;
            }
            
            switch (match.Groups["InAir"].Value)
            {
                case "true":
                    return airCheck.GetInAir();
                case "false":
                    return !airCheck.GetInAir();
                default:
                    Log.Error("does not match inAir!");
                    return false;
            }
        }
    }
}