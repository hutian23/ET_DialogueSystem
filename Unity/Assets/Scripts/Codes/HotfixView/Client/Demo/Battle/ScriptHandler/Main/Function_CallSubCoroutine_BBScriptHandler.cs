using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    public class Function_CallSubCoroutine_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CallSubCoroutine";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CallSubCoroutine: (?<FunctionName>\w+);");

            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BehaviorInfo info = machine.GetInfoByOrder(machine.GetCurrentOrder());

            parser.Invoke(parser.GetFunctionPointer(info.behaviorName, match.Groups["FunctionName"].Value), token).Coroutine();


            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}