using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof (b2Body))]
    public class AfterB2WorldCreate_CreateGroundBody: AEvent<AfterB2WorldCreated>
    {
        protected override async ETTask Run(Scene scene, AfterB2WorldCreated args)
        {
            World World = args.B2World.World;

            //ground
            var groundBodyDef = new BodyDef { BodyType = BodyType.StaticBody, Position = Vector2.Zero };
            var groundBody = World.CreateBody(groundBodyDef);
            var groundBox = new PolygonShape();
            groundBox.SetAsBox(50f, 2.0f);
            groundBody.CreateFixture(groundBox, 0.0f);
            
            await ETTask.CompletedTask;
        }
    }
}