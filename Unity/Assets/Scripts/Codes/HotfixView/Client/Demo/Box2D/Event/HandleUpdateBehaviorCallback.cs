using System.Numerics;
using Box2DSharp.Dynamics;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    public class HandleUpdateBehaviorCallback: AInvokeHandler<UpdateBehaviorCallback>
    {
        public override void Handle(UpdateBehaviorCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body b2body = b2GameManager.Instance.GetBody(unit.InstanceId);

            //1. Destroy old fixture
            for (int i = 0; i < b2body.fixtures.Count; i++)
            {
                Fixture fixture = b2body.fixtures[i];
                b2body.body.DestroyFixture(fixture);
            }
            b2body.fixtures.Clear();
            b2body.frame = 0;
            b2body.velocity = Vector2.Zero;
            b2body.totalPos = Vector2.Zero;
            
            //2. update behavior
            timelineComponent.Reload(args.behaviorOrder);
        }
    }
}