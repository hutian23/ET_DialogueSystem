using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    // [FriendOf(typeof (BehaviorInfo))]
    public class SkillOrder_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillOrder";
        }

        //SkillOrder: Normal,30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SkillOrder: (?<skill>\w+),(?<order>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string skillType = match.Groups["skill"].Value;
            switch (skillType)
            {
                case "Move":
                    break;
                case "Normal":
                    break;
                case "SpecialMove":
                    break;
                case "SuperArt":
                    break;
            }

            // int.TryParse(match.Groups["order"].Value, out int order);
            // BehaviorInfo behaviorInfo = parser.GetParent<DialogueComponent>().GetComponent<BBInputComponent>().GetSkillInfo(parser.currentID);
            // skillInfo.order = order;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}