using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BBSkillInfo))]
    public class SkillType_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillType";
        }

        //SkillType: Normal
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            BBParser bbParser = unit.GetComponent<DialogueComponent>().GetComponent<BBParser>();

            Match match = Regex.Match(opCode, @"SkillType: (?<skill>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }

            string skillType = match.Groups["skill"].Value;
            BBSkillInfo skillInfo = FTGHelper.GetSkillInfo(unit, bbParser.currentID);
            switch (skillType)
            {
                case "Move":
                    skillInfo.skillType = SkillOrder.Move;
                    break;
                case "Normal":
                    skillInfo.skillType = SkillOrder.Normal;
                    break;
                case "SpecialMove":
                    skillInfo.skillType = SkillOrder.SpecialMove;
                    break;
                case "SuperArt":
                    skillInfo.skillType = SkillOrder.SuperArt;
                    break;
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}