using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    [FriendOf(typeof (HitboxComponent))]
    public class HandleUpdateFlipCallback: AInvokeHandler<UpdateFlipCallback>
    {
        public override void Handle(UpdateFlipCallback args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();

            b2Body b2Body = b2GameManager.Instance.GetBody(args.instanceId);
            //1. Dispose old fixtures
            for (int i = 0; i < b2Body.fixtures.Count; i++)
            {
                Fixture fixture = b2Body.fixtures[i];
                b2Body.body.DestroyFixture(fixture);
            }

            b2Body.fixtures.Clear();

            //2. Update fixtures
            foreach (BoxInfo info in hitboxComponent.keyFrame.boxInfos)
            {
                PolygonShape shape = new();
                shape.SetAsBox(info.size.x / 2, info.size.y / 2, new Vector2(info.center.x * b2Body.GetFlip(), info.center.y), 0f);
                FixtureDef fixtureDef = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0.3f,
                    UserData = info,
                    IsSensor = info.hitboxType is not HitboxType.Squash
                };
                Fixture fixture = b2Body.body.CreateFixture(fixtureDef);
                b2Body.fixtures.Add(fixture);
            }
        }
    }
}