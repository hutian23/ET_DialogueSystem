using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorBufferComponent))]
    [FriendOf(typeof(TriggerCheck))]
    public class RegistTriggerCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistTrigger";
        }

        //RegistTrigger: 5;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistTrigger: (?<lastedFrame>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            BehaviorBufferComponent bufferComponent = parser.GetParent<DialogueComponent>().GetComponent<BehaviorBufferComponent>();
            //存在相同组件
            if (bufferComponent.triggerCheckDict.ContainsKey(data.targetID))
            {
                Log.Error($"already contain triggerCheck: {data.targetID}");
                return Status.Failed;
            }

            TriggerCheck triggerCheck = bufferComponent.AddChild<TriggerCheck>();
            bufferComponent.triggerCheckDict.Add(data.targetID, triggerCheck);
            triggerCheck.targetID = data.targetID;
            if (!long.TryParse(match.Groups["lastedFrame"].Value, out long lastedFrame))
            {
                Log.Error($"cannot convert {match.Groups["lastedFrame"]} to int!!!");
                return Status.Failed;
            }

            triggerCheck.lastedFrame = lastedFrame;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}