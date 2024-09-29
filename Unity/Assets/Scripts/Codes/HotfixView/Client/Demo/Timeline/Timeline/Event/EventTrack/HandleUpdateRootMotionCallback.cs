using Box2DSharp.Testbed.Unity.Inspection;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    public class HandleUpdateRootMotionCallback: AInvokeHandler<UpdateRootMotionCallback>
    {
        public override void Handle(UpdateRootMotionCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body body = b2GameManager.Instance.GetBody(unit.InstanceId);
            body.velocity = args.velocity.ToVector2();
            body.totalPos += args.velocity.ToVector2();
            body.frame++;
        }
    }
}