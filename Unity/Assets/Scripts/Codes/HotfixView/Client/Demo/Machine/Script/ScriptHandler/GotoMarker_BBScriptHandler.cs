using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    public class GotoMarker_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GotoMarker";
        }

        //GotoMarker: 'Loop';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "GotoMarker: '(?<marker>.*?)';");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            BehaviorMachine machine = parser.GetParent<Unit>().GetComponent<BehaviorMachine>();
            BehaviorInfo info = machine.GetInfoByOrder(machine.GetCurrentOrder());

            int pointer = parser.GetMarkerPointer(info.behaviorName, match.Groups["marker"].Value);
            parser.Coroutine_Pointers[data.CoroutineID] = pointer;

            //这里防止卡死
            await TimerComponent.Instance.WaitFrameAsync(token);
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}