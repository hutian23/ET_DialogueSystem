using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_EnemyFlipChange_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "EnemyFlipChange";
        }

        //EnemyFlipChange: false;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"EnemyFlipChange: (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            EnemyFlipCheckComponent component = parser.GetComponent<EnemyFlipCheckComponent>();
            switch (match.Groups["Active"].Value)
            {
                case "true":
                    return component.GetFlipChange();
                case "false":
                    return !component.GetFlipChange();
                default:
                    Log.Error($"matched failed");
                    return false;
            }
        }
    }
}