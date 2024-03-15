using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    public class RegistSkillTag_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillTag";
        }

        //SkillTag: 'Sol_GunFlame';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SkillTag: '(?<tag>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            BehaviorInfo info = FTGHelper.GetBehaviorInfo(parser, data.targetID);
            info.tag = match.Groups["tag"].Value;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}