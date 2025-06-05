using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_Accessible_BBScriptHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Accessible";
        }

        // 比如普攻连段 1A 2A 3A, 2A 在默认情况下不能进入
        // Accessible: Active;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Accessible: (?<Active>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            return match.Groups["Active"].Value.Equals("true");
        }
    }
}