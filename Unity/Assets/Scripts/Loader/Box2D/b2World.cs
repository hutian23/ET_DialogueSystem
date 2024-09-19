using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Testbed.Abstractions;

namespace ET
{
    public class b2World: TestBase
    {
        public b2World(b2Game _Game)
        {
            //render
            _Game.PreRenderCallback += Drawb2World;

            //Load
            Input = Global.Input;
            Draw = Global.DebugDraw;
            TestSettings = Global.Settings;
            World.Draw = Global.DebugDraw;

            //ground
            //1. 创建刚体
            var groundBodyDef = new BodyDef { BodyType = BodyType.StaticBody, Position = Vector2.Zero };
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

        private void Drawb2World()
        {
            DrawFlag flags = 0;
            if (TestSettings.DrawShapes)
            {
                flags |= DrawFlag.DrawShape;
            }

            if (TestSettings.DrawJoints)
            {
                flags |= DrawFlag.DrawJoint;
            }

            if (TestSettings.DrawAABBs)
            {
                flags |= DrawFlag.DrawAABB;
            }

            if (TestSettings.DrawCOMs)
            {
                flags |= DrawFlag.DrawCenterOfMass;
            }

            if (TestSettings.DrawContactPoints)
            {
                flags |= DrawFlag.DrawContactPoint;
            }

            Draw.Flags = flags;
            World.DebugDraw();
            if (TestSettings.DrawContactPoints)
            {
                const float ImpulseScale = 0.1f;
                const float AxisScale = 0.3f;
                for (var i = 0; i < PointsCount; ++i)
                {
                    var point = Points[i];
                    if (point.State == PointState.AddState)
                    {
                        // Add
                        Draw.DrawPoint(point.Position, 10f, Color.FromArgb(77, 242, 77));
                    }
                    else if (point.State == PointState.PersistState)
                    {
                        // Persist
                        Draw.DrawPoint(point.Position, 5f, Color.FromArgb(77, 77, 242));
                    }

                    if (TestSettings.DrawContactNormals)
                    {
                        var p1 = point.Position;
                        var p2 = p1 + AxisScale * point.Normal;
                        Draw.DrawSegment(p1, p2, Color.FromArgb(230, 230, 230));
                    }
                    else if (TestSettings.DrawContactImpulse)
                    {
                        var p1 = point.Position;
                        var p2 = p1 + ImpulseScale * point.NormalImpulse * point.Normal;
                        Global.DebugDraw.DrawSegment(p1, p2, Color.FromArgb(230, 230, 77));
                    }

                    if (TestSettings.DrawFrictionImpulse)
                    {
                        var tangent = MathUtils.Cross(point.Normal, 1.0f);
                        var p1 = point.Position;
                        var p2 = p1 + ImpulseScale * point.TangentImpulse * tangent;
                        Draw.DrawSegment(p1, p2, Color.FromArgb(230, 230, 77));
                    }
                }
            }

            if (BombSpawning)
            {
                Draw.DrawPoint(BombSpawnPoint, 4.0f, Color.Blue);
                Draw.DrawSegment(MouseWorld, BombSpawnPoint, Color.FromArgb(203, 203, 203));
            }
        }
    }
}