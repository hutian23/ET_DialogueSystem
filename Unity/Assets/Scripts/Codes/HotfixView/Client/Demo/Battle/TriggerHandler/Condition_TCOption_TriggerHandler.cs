using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_TCOption_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "TCOption";
        }

        //TCOption: BehaviorName, BuffFrame;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"TCOption: (?<Option>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            TargetCancelComponent tc = parser.GetComponent<TargetCancelComponent>();
            return tc != null && tc.Contain(match.Groups["Option"].Value) ;
        }
    }
}