using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(BehaviorInfo))]
    public class HitEvent_HitStun_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStun";
        }
        
        //Hit_GotoState: KnockBack;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitStun: (?<hitFlag>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            CollisionInfo info = parser.GetComponent<HitComponent>().GetInfo();
            
            b2Body _body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            Unit _unit = _body.GetParent<Unit>();
            BehaviorMachine _machine = _unit.GetComponent<BehaviorMachine>();
            BehaviorInfo _info = _machine.GetInfoByFlag(match.Groups["hitFlag"].Value);
            
            _machine.Reload(_info.behaviorOrder);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}