using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(BehaviorInfo))]
    public class HitNotify_HitStun_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStun";
        }

        //代码块中可执行，需要注册Hurt_CollisionInfo变量
        //Hit_GotoState: 'KnockBack';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitStun: (?<hitFlag>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            CollisionInfo info = parser.GetParam<CollisionInfo>("HitNotify_CollisionInfo");
            b2Body body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(body.unitId) as Unit;
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();

            BehaviorInfo _info = machine.GetInfoByFlag(match.Groups["hitFlag"].Value);
            machine.Reload(_info.behaviorOrder);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}