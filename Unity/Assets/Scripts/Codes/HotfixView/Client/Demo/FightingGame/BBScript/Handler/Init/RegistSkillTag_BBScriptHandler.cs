using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBInputComponent))]
    public class RegistSkillTag_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistSkillTag";
        }

        //RegistSkillTag tag = Sol_GunFlame;
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            Match match = Regex.Match(opCode, @"RegistSkillTag tag = (?<SkillTag>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }
            
            BBInputComponent bbInput = unit.GetComponent<DialogueComponent>().GetComponent<BBInputComponent>();
            bbInput.skillMap.TryAdd(match.Groups["SkillTag"].Value, bbInput.currentID);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}