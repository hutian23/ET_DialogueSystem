using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
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

            //1. Dispose old hitboxFixtures
            for (int i = 0; i < b2Body.hitboxFixtures.Count; i++)
            {
                Fixture fixture = b2Body.hitboxFixtures[i];
                b2Body.body.DestroyFixture(fixture);
            }

            b2Body.hitboxFixtures.Clear();
            //2. update hitboxFixtures
            foreach (BoxInfo info in args.Keyframe.boxInfos)
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
                b2Body.hitboxFixtures.Add(fixture);
            }
        }
    }
}