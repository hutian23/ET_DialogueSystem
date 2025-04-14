using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class HitNotify_HitParam_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitParam";
        }

        //代码块中可执行，需要注册Hurt_CollisionInfo变量
        //HitParam: StopFrame, 10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitParam: (?<ParamName>.*?), (?<ParamValue>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            CollisionInfo info = parser.GetParam<CollisionInfo>("HitNotify_CollisionInfo");
            b2Body body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(body.unitId) as Unit;
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();

            machine.TryRemoveTmpParam(match.Groups["ParamName"].Value);
            machine.RegistTmpParam(match.Groups["ParamName"].Value, match.Groups["ParamValue"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}