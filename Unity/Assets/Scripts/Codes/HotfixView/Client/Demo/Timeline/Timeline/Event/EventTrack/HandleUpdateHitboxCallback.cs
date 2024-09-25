using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Box2DSharp.Testbed.Unity.Inspection;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (HitboxComponent))]
    [FriendOf(typeof (b2Body))]
    public class HandleUpdateHitboxCallback: AInvokeHandler<UpdateHitboxCallback>
    {
        public override void Handle(UpdateHitboxCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
            hitboxComponent.keyFrame = args.Keyframe;

            long unitId = timelineComponent.GetParent<Unit>().InstanceId;
            b2Body b2Body = b2GameManager.Instance.GetBody(unitId);

            //1. Dispose old fixtures
            for (int i = 0; i < b2Body.body.FixtureList.Count; i++)
            {
                b2Body.body.DestroyFixture(b2Body.body.FixtureList[i]);
            }

            //2. update fixtures
            foreach (BoxInfo info in args.Keyframe.boxInfos)
            {
                PolygonShape shape = new();
                shape.SetAsBox(info.size.x / 2, info.size.y / 2, info.center.ToVector2(), 0);
                FixtureDef fixtureDef = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0.3f,
                    UserData = info,
                    IsSensor = info.hitboxType is not HitboxType.Squash
                };

                b2Body.body.CreateFixture(fixtureDef);
            }
        }
    }
}