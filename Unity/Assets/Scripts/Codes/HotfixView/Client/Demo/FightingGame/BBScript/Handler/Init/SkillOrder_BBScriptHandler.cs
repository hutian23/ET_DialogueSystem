using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.BBParser))]
    public class SkillOrder_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillOrder";
        }

        //Order: 30;
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
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}