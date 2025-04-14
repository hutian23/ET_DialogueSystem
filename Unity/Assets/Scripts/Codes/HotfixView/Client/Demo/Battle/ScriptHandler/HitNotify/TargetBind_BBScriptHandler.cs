using ET.Event;

namespace ET.Client
{
    public class TargetBind_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TargetBind";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            CollisionInfo info = parser.GetParam<CollisionInfo>("HitNotify_CollisionInfo");
            b2Body bodyB = Root.Instance.Get(info.dataB.InstanceId) as b2Body;

            parser.TryRemoveParam("TargetBind");
            parser.RegistParam("TargetBind", bodyB.InstanceId);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}