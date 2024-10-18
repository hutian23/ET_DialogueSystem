using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (SkillBuffer))]
    [FriendOf(typeof (SkillInfo))]
    [FriendOf(typeof (CancelManager))]
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
            CancelManager cancelManager = parser.GetParent<TimelineComponent>().GetComponent<CancelManager>();
            foreach (var kv in buffer.infoDict)
            {
                SkillInfo info = buffer.GetChild<SkillInfo>(kv.Value);
                if (info.behaviorName.Equals(match.Groups["Option"].Value))
                {
                    cancelManager.GcOptions.Add(info.behaviorOrder);
                    return Status.Success;
                }
            }

            Log.Error($"does not exist behavior: {match.Groups["Option"].Value}");
            await ETTask.CompletedTask;
            return Status.Failed;
        }
    }
}