using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_FindTarget_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "FindTarget";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"FindTarget: (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            TargetCheckComponent component = parser.GetComponent<TargetCheckComponent>();
            switch (match.Groups["Active"].Value)
            {
                case "true":
                    return component.FindTarget();
                case "false":
                    return !component.FindTarget();
                default:
                    Log.Error($"matched failed");
                    return false;
            }
        }
    }
}