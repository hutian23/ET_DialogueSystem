using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (SkillInfo))]
    public class BehaviorName_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Name";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Name: '(?<BehaviorName>\w+)'");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            SkillInfo info = buffer.GetInfo(parser.GetParam<int>("BehaviorOrder"));
            info.behaviorName = match.Groups["BehaviorName"].Value;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}