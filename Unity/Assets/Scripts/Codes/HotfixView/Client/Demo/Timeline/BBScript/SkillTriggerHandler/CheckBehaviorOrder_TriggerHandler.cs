using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckBehaviorOrder_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "BehaviorOrder";
        }

        //BehaviorOrder: 0;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"BehaviorOrder: (?<Order>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }

            int.TryParse(match.Groups["Order"].Value, out int order);

            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            return buffer.GetCurrentOrder() == order;
        }
    }
}