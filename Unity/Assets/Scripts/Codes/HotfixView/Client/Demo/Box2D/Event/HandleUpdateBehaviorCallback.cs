
namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    public class HandleUpdateBehaviorCallback : AInvokeHandler<UpdateBehaviorCallback>
    {
        public override void Handle(UpdateBehaviorCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body b2body = b2GameManager.Instance.GetBody(unit.InstanceId);

            //1. Destroy old fixture
            for (int i = 0; i < b2body.body.FixtureList.Count; i++)
            {
                b2body.body.DestroyFixture(b2body.body.FixtureList[i]);
            }
            
            //2. update behavior
            timelineComponent.Reload(args.behaviorOrder);
        }
    }
}