using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Testbed.Abstractions;

namespace ET
{
    [TestCase("Learning","HelloWorld")]
    public class Learning_HelloWorld : TestBase
    {
        private Body _body;

        public Learning_HelloWorld()
        {
            //ground
            //1. 创建刚体
            var groundBodyDef = new BodyDef
            {
                BodyType = BodyType.StaticBody,
                Position = Vector2.Zero
            };
            var groundBody = World.CreateBody(groundBodyDef);

            //2. 夹具
            var groundBox = new PolygonShape();
            groundBox.SetAsBox(50f, 5.0f);
            groundBody.CreateFixture(groundBox, 0.0f);
            
            //dynamic body
            //1. bodyDef
            var bodyDef = new BodyDef
            {
                BodyType = BodyType.DynamicBody,
                Position = new Vector2(0,10f),
                FixedRotation = true
            };
            //2. shape
            var dynamicBox = new PolygonShape();
            dynamicBox.SetAsBox(1f, 1f, Vector2.Zero, 45f);
            //3. fixture
            var fixtureDef = new FixtureDef { Shape = dynamicBox, Density = 1.0f, Friction = 0.3f };

            var body = World.CreateBody(bodyDef);
            body.CreateFixture(fixtureDef);
        }
    }
}
