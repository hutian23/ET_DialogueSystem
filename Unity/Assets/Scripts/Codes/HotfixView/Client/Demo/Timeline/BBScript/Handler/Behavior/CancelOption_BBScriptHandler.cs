using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (SkillBuffer))]
    [FriendOf(typeof (SkillInfo))]
    public class AddCancelOption_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AddCancelOption";
        }

        //AddCancelOption: '';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AddCancelOption: '(?<Option>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //string behaviorName ---> BehaviorOrder
            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            buffer.AddGCOption(match.Groups["Option"].Value);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}