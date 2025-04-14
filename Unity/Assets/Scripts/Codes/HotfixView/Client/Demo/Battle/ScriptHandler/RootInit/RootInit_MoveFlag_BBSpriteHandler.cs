using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorMachine))]
    public class RootInit_MoveFlag_BBSpriteHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MoveFlag";
        }

        //MoveFlag: Hurt1;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"MoveFlag: (?<MoveFlag>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            BehaviorMachine machine = parser.GetParent<Unit>().GetComponent<BehaviorMachine>();
            BehaviorInfo info = machine.GetChild<BehaviorInfo>(parser.GetParam<long>("InfoId"));
            machine.behaviorFlagDict.TryAdd(match.Groups["MoveFlag"].Value, info.Id);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}