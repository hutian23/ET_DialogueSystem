using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(SkillInfo))]
    public class BehaviorOrder_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Order";
        }

        //BehaviorOrder: 0;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Order: (?<BehaviorOrder>\w+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["BehaviorOrder"].Value, out int behaviorOrder))
            {
                Log.Error($"cannot parse {match.Groups["BehaviorOrder"].Value} to int");
                return Status.Failed;
            }

            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            SkillInfo info = buffer.GetChild<SkillInfo>(parser.GetParam<long>("InfoId"));
            info.behaviorOrder = behaviorOrder;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}