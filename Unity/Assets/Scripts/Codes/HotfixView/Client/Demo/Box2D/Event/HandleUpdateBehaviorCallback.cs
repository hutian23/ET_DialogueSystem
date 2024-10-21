using Box2DSharp.Dynamics;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    public class HandleUpdateBehaviorCallback: AInvokeHandler<UpdateBehaviorCallback>
    {
        public override void Handle(UpdateBehaviorCallback args)
        {
            // TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            // Unit unit = timelineComponent.GetParent<Unit>();
            // b2Body b2body = b2GameManager.Instance.GetBody(unit.InstanceId);
            //
            // //1. Destroy old fixture
            // for (int i = 0; i < b2body.hitboxFixtures.Count; i++)
            // {
            //     Fixture fixture = b2body.hitboxFixtures[i];
            //     b2body.body.DestroyFixture(fixture);
            // }
            // b2body.hitboxFixtures.Clear();
            //
            // //2. update behavior
            // timelineComponent.Reload(args.behaviorOrder);
        }
    }
}