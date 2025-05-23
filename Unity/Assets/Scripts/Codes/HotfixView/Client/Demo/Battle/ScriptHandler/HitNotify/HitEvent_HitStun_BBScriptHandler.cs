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

            //1. 查询组件
            CollisionBuffer buffer = parser.GetComponent<HitComponent>().GetBuffer();
            b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
            b2Body bodyB = boxB.GetParent<b2Body>();
            Unit unitB = Root.Instance.Get(bodyB.unitId) as Unit;
            BehaviorMachine machine = unitB.GetComponent<BehaviorMachine>();
            
            //2. 动作切换
            BehaviorInfo info = machine.GetInfoByFlag(match.Groups["hitFlag"].Value); // RegistMove时，需要给Behavior添加对应的moveFlag
            machine.Reload(info.behaviorOrder);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}