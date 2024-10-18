using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckTransition_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Transition";
        }

        //Transition: 'RunToIdle';
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Transition: '(?<transition>\w+)'");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            return parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>().ContainTransition(match.Groups["transition"].Value);
        }
    }
}