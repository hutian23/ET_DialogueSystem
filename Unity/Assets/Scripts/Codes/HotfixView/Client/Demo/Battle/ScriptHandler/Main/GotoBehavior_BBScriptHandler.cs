using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BehaviorMachine))]
    public class GotoBehavior_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GotoBehavior";
        }

        //GotoBehavior: 'Mai_LandBounce';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"GotoBehavior: '(?<behavior>.*?)';");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            BehaviorMachine machine = parser.GetParent<Unit>().GetComponent<BehaviorMachine>();
            machine.Reload(match.Groups["behavior"].Value);

            await ETTask.CompletedTask;
            return Status.Return;
        }
    }
}