using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Box2DSharp.Testbed.Unity.Inspection;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof (b2Body))]
    public class AfterB2WorldCreate_HelloWorldView: AEvent<AfterB2WorldCreate>
    {
        protected override async ETTask Run(Scene scene, AfterB2WorldCreate args)
        {
            World World = args.B2World.World;

            //ground
            var groundBodyDef = new BodyDef { BodyType = BodyType.StaticBody, Position = Vector2.Zero };
            var groundBody = World.CreateBody(groundBodyDef);
            var groundBox = new PolygonShape();
            groundBox.SetAsBox(50f, 5.0f);
            groundBody.CreateFixture(groundBox, 0.0f);

            //1. Player
            Unit player = TODUnitHelper.GetPlayer(scene.ClientScene());
            
            var bodyDef = new BodyDef
            {
                BodyType = BodyType.DynamicBody, Position = new Vector2(0, 15), Angle = Utils.GetRadian(0f)
            };
            var body = World.CreateBody(bodyDef);

            HitboxKeyframe keyframe = null;
            TimelinePlayer timelinePlayer = player.GetComponent<TimelineComponent>().GetTimelinePlayer();
            foreach (var track in timelinePlayer.CurrentTimeline.Tracks)
            {
                if (track is BBHitboxTrack hitboxTrack)
                {
                    keyframe = hitboxTrack.GetKeyframe(0);
                }
            }

            foreach (var boxInfo in keyframe.boxInfos)
            {
                var box = new PolygonShape();
                box.SetAsHitbox(boxInfo);
                body.CreateFixture(box, 1.0f);
                b2Body B2Body = b2GameManager.Instance.AddChild<b2Body>();
                B2Body.IsPlayer = true;
                B2Body.body = body;
                B2Body.unitId = player.InstanceId;   
            }

            await ETTask.CompletedTask;
        }
    }
}