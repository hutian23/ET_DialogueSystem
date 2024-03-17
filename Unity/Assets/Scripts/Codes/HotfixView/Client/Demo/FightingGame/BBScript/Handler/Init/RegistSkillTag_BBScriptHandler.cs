using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (BehaviorInfo))]
    [FriendOf(typeof (BehaviorBufferComponent))]
    public class RegistSkillTag_BBScriptHandler: BBScriptHandler
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

            BehaviorBufferComponent bufferComponent = parser.GetParent<DialogueComponent>().GetComponent<BehaviorBufferComponent>();
            if (!bufferComponent.tagDict.TryAdd(info.tag, info.targetID))
            {
                Log.Error($"already contain tag: {info.tag}");
                return Status.Failed;
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}