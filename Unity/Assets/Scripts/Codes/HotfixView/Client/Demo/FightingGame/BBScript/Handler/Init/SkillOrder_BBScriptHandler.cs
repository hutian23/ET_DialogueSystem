using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (BBSkillInfo))]
    public class SkillOrder_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Order";
        }

        //Order: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Order: (?<order>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            uint.TryParse(match.Groups["order"].Value, out uint order);
            BBSkillInfo skillInfo = parser.GetParent<DialogueComponent>().GetComponent<BBInputComponent>().GetSkillInfo(parser.currentID);
            skillInfo.order = order;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}