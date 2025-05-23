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
            CollisionBuffer buffer = parser.GetComponent<HitComponent>().GetBuffer();
            b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
            b2Body bodyB = boxB.GetParent<b2Body>();
            Unit unitB = Root.Instance.Get(bodyB.unitId) as Unit;
            
            parser.TryRemoveParam("TargetBind");
            parser.RegistParam("TargetBind", unitB.InstanceId);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}