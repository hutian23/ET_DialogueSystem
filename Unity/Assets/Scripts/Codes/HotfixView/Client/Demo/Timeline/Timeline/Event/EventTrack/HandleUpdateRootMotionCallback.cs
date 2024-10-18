using Box2DSharp.Testbed.Unity.Inspection;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    [FriendOf(typeof (RootMotionComponent))]
    public class HandleUpdateRootMotionCallback: AInvokeHandler<UpdateRootMotionCallback>
    {
        public override void Handle(UpdateRootMotionCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body B2body = b2GameManager.Instance.GetBody(unit.InstanceId);
            if (args.ApplyRootMotion)
            {
                RootMotionComponent rootMotion = B2body.GetComponent<RootMotionComponent>() ?? B2body.AddComponent<RootMotionComponent>();
                rootMotion.velocity = args.velocity.ToVector2();
                B2body.body.SetLinearVelocity(rootMotion.velocity * B2body.GetFlip());
            }
            else
            {
                B2body.RemoveComponent<RootMotionComponent>();
            }
        }
    }
}