using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (BehaviorInfo))]
    [FriendOf(typeof (BehaviorBufferComponent))]
    public class SkillOrder_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillOrder";
        }

        //SkillOrder: 0;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //Find node
            DialogueComponent dialogueComponent = parser.GetParent<DialogueComponent>();
            BBNode node = dialogueComponent.GetNode(data.targetID) as BBNode;

            Match match = Regex.Match(data.opLine, @"SkillOrder: (?<order>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //update behavior info
            string skillOrder = match.Groups["order"].Value;
            uint.TryParse(skillOrder, out uint order);
            BehaviorInfo info = FTGHelper.GetBehaviorInfo(parser, data.targetID);
            info.order = order;
            info.behaviorName = node.behaviorName;

            BehaviorBufferComponent bufferComponent = parser.GetParent<DialogueComponent>().GetComponent<BehaviorBufferComponent>();
            if (!bufferComponent.orderDict.TryAdd(info.GetOrder(), info.targetID))
            {
                Log.Error($"already contain order: {info.GetOrder()}");
                return Status.Failed;
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}