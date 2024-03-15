using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    public class SkillOrder_BBScriptHandler : BBScriptHandler
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
            uint skill = 0;
            switch (skillType)
            {
                case "Move":
                    skill = SkillOrder.Move;
                    break;
                case "Normal":
                    skill = SkillOrder.Normal;
                    break;
                case "SpecialMove":
                    skill = SkillOrder.SpecialMove;
                    break;
                case "SuperArt":
                    skill = SkillOrder.SuperArt;
                    break;
            }
            
            uint.TryParse(match.Groups["order"].Value, out uint order);
            BehaviorInfo info = FTGHelper.GetBehaviorInfo(parser, data.targetID);
            info.order = order;
            info.skillType = skill;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}