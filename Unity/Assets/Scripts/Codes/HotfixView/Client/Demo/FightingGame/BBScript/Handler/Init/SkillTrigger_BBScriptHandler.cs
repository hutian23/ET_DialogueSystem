using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    // [FriendOf(typeof(BehaviorInfo))]
    public class SkillTrigger_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillTrigger";
        }

        //SkillTrigger: HP < 10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // Match match = Regex.Match(data.opLine, @":\s*(.+);");
            // if (!match.Success)
            // {
            //     DialogueHelper.ScripMatchError(data.opLine);
            //     return Status.Failed;
            // }
            //
            // BehaviorInfo skillInfo = parser.GetParent<DialogueComponent>().GetComponent<BBInputComponent>().GetSkillInfo(parser.currentID);
            // skillInfo.triggers.Add(match.Groups[1].Value);
            //
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}