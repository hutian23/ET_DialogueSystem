using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    // [FriendOf(typeof(BehaviorInfo))]
    public class SkillType_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillType";
        }

        //SkillType: Normal;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // Match match = Regex.Match(data.opLine, @"SkillType: (?<skill>\w+);");
            // if (!match.Success)
            // {
            //     DialogueHelper.ScripMatchError(data.opLine);
            //     return Status.Failed;
            // }
            //
            // string skillType = match.Groups["skill"].Value;
            // BehaviorInfo skillInfo = parser.GetParent<DialogueComponent>().GetComponent<BBInputComponent>().GetSkillInfo(parser.currentID);
            // switch (skillType)
            // {
            //     case "Move":
            //         skillInfo.BehaviorType = SkillOrder.Move;
            //         break;
            //     case "Normal":
            //         skillInfo.BehaviorType = SkillOrder.Normal;
            //         break;
            //     case "SpecialMove":
            //         skillInfo.BehaviorType = SkillOrder.SpecialMove;
            //         break;
            //     case "SuperArt":
            //         skillInfo.BehaviorType = SkillOrder.SuperArt;
            //         break;
            // }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}