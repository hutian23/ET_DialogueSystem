using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class HitEvent_TargetBind_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TargetBind";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            CollisionInfo info = parser.GetComponent<HitComponent>().GetInfo();
            b2Body bodyB = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            Unit unitB = bodyB.GetParent<Unit>();

            parser.TryRemoveParam("TargetBind");
            parser.RegistParam("TargetBind", unitB.InstanceId);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}