using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class HitEvent_HitFlag_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitFlag";
        }

        //代码块中可执行，需要注册Hurt_CollisionInfo变量
        //HitParam: StopFrame;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitParam: (?<Flag>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            CollisionInfo info = parser.GetComponent<HitComponent>().GetInfo();

            b2Body _body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            Unit _unit = _body.GetParent<Unit>();
            BBParser _parser = _unit.GetComponent<BBParser>();

            _parser.RegistParam(match.Groups["Flag"].Value, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}