using System;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Testbed.Abstractions;
using Timeline;
using UnityEngine;
using Color = Box2DSharp.Common.Color;
using Transform = Box2DSharp.Common.Transform;
using Vector2 = System.Numerics.Vector2;

namespace ET
{
    public class b2World: TestBase
    {
        private b2Game Game;

        public b2World(b2Game _Game)
        {
            Game = _Game;
            //render
            Game.PreRenderCallback += Drawb2World;

            //Load
            Input = Global.Input;
            Draw = Global.DebugDraw;
            TestSettings = Global.Settings;
            World.Draw = Global.DebugDraw;
        }

        public override void Dispose()
        {
            Game.PreRenderCallback -= Drawb2World;
            Game = null;
        }

        public new void Step()
        {
            TimeStep = this.TestSettings.Hertz > 0.0f? 1.0f / TestSettings.Hertz : 0f;
            // if (this.TestSettings.Pause)
            // {
            //     this.TimeStep = this.TestSettings.SingleStep? 1f : 0f;
            // }

            this.World.AllowSleep = this.TestSettings.EnableSleep;
            this.World.WarmStarting = this.TestSettings.EnableWarmStarting;
            this.World.SubStepping = this.TestSettings.EnableSubStepping;

            this.PointsCount = 0;
            
            this.PreStep();
            this.World.Step(this.TimeStep,this.TestSettings.VelocityIterations,this.TestSettings.PositionIterations);
            this.PostStep();
        }

        #region Render

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
            // World.DebugDraw();
            DebugDraw();
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

        private void DebugDraw()
        {
            if (World.Draw == null)
            {
                return;
            }

            var flags = Draw.Flags;

            if (flags.IsSet(DrawFlag.DrawShape))
            {
                for (var node = World.BodyList.First; node != null; node = node.Next)
                {
                    var b = node.Value;
                    var xf = b.GetTransform();
                    var isEnabled = b.IsEnabled;
                    var isAwake = b.IsAwake;
                    //绘制夹具形状
                    foreach (var f in b.Fixtures)
                    {
                        if (b.BodyType == BodyType.DynamicBody && b.Mass.Equals(0))
                        {
                            DrawShape(f, xf);
                        }

                        else if (!isEnabled)
                        {
                            DrawShape(f, xf);
                        }

                        else if (b.BodyType == BodyType.StaticBody)
                        {
                            DrawShape(f, xf);
                        }
                        else if (b.BodyType == BodyType.KinematicBody)
                        {
                            DrawShape(f, xf);
                        }
                        else if (!isAwake)
                        {
                            DrawShape(f, xf);
                        }
                        else
                        {
                            DrawShape(f, xf);
                        }
                    }
                }
            }

            if (flags.IsSet(DrawFlag.DrawJoint))
            {
                var node = World.JointList.First;
                while (node != null)
                {
                    node.Value.Draw(World.Draw);
                    node = node.Next;
                }
            }

            if (flags.IsSet(DrawFlag.DrawPair))
            {
                var color = Color.FromArgb(77, 230, 230);
                for (var node = World.ContactManager.ContactList.First; node != null; node = node.Next)
                {
                    var c = node.Value;
                    var fixtureA = c.FixtureA;
                    var fixtureB = c.FixtureB;

                    var cA = fixtureA.GetAABB(c.ChildIndexA).GetCenter();
                    var cB = fixtureB.GetAABB(c.ChildIndexB).GetCenter();

                    World.Draw.DrawSegment(cA, cB, color);
                }
            }

            if (flags.IsSet(DrawFlag.DrawAABB))
            {
                var color = Color.FromArgb(230, 77, 230);
                var bp = World.ContactManager.BroadPhase;

                var node = World.BodyList.First;
                Span<Vector2> vs = stackalloc Vector2[4];
                while (node != null)
                {
                    var b = node.Value;
                    node = node.Next;
                    if (b.IsEnabled)
                    {
                        continue;
                    }

                    foreach (var f in b.Fixtures)
                    {
                        foreach (var proxy in f.Proxies)
                        {
                            var aabb = bp.GetFatAABB(proxy.ProxyId);
                            vs[0] = new Vector2(aabb.LowerBound.X, aabb.LowerBound.Y);
                            vs[1] = new Vector2(aabb.UpperBound.X, aabb.LowerBound.Y);
                            vs[2] = new Vector2(aabb.UpperBound.X, aabb.UpperBound.Y);
                            vs[3] = new Vector2(aabb.LowerBound.X, aabb.UpperBound.Y);
                            World.Draw.DrawPolygon(vs, 4, color);
                        }
                    }
                }
            }

            if (flags.IsSet(DrawFlag.DrawCenterOfMass))
            {
                var node = World.BodyList.First;
                while (node != null)
                {
                    var b = node.Value;
                    node = node.Next;
                    var xf = b.GetTransform();
                    xf.Position = b.GetWorldCenter();
                    World.Draw.DrawTransform(xf);
                }
            }
        }

        private void DrawShape(Fixture fixture, in Transform xf)
        {
            var defaultColor = Color.White;
            switch (fixture.Shape)
            {
                case CircleShape circle:
                {
                    var center = MathUtils.Mul(xf, circle.Position);
                    var radius = circle.Radius;
                    //Calculate rotation
                    var axis = MathUtils.Mul(xf.Rotation, new Vector2(1.0f, 0.0f));
                    World.Draw.DrawSolidCircle(center, radius, axis, defaultColor);
                    break;
                }
                case EdgeShape edge:
                {
                    var v1 = MathUtils.Mul(xf, edge.Vertex1);
                    var v2 = MathUtils.Mul(xf, edge.Vertex2);
                    World.Draw.DrawSegment(v1, v2, defaultColor);

                    if (!edge.OneSided)
                    {
                        World.Draw.DrawPoint(v1, 4.0f, defaultColor);
                        World.Draw.DrawPoint(v2, 4.0f, defaultColor);
                    }

                    break;
                }
                case ChainShape chain:
                {
                    var count = chain.Count;
                    var vertices = chain.Vertices;

                    var v1 = MathUtils.Mul(xf, vertices[0]);
                    for (int i = 0; i < count; i++)
                    {
                        var v2 = MathUtils.Mul(xf, vertices[i]);
                        World.Draw.DrawSegment(v1, v2, defaultColor);
                        v1 = v2;
                    }

                    break;
                }
                case PolygonShape poly:
                {
                    var vertexCount = poly.Count;
                    Debug.Assert(vertexCount <= Settings.MaxPolygonVertices);
                    Span<Vector2> vertices = stackalloc Vector2[vertexCount];

                    for (var i = 0; i < vertexCount; i++)
                    {
                        vertices[i] = MathUtils.Mul(xf, poly.Vertices[i]);
                    }

                    var color = defaultColor;
                    BoxInfo info = fixture.UserData as BoxInfo;
                    switch (info?.hitboxType)
                    {
                        case HitboxType.Hit:
                            color = Global.Settings.ShowHitbox? Color.Red : Color.Transparent;
                            break;
                        case HitboxType.Hurt:
                            color = Global.Settings.ShowHurtBox? Color.Green : Color.Transparent;
                            break;
                        case HitboxType.Squash:
                            color = Global.Settings.ShowSquashBox? Color.Yellow : Color.Transparent;
                            break;
                        case HitboxType.Throw:
                            color = Global.Settings.ShowThrowBox? Color.Blue : Color.Transparent;
                            break;
                        case HitboxType.Proximity:
                            color = Global.Settings.ShowProximityBox? Color.Magenta : Color.Transparent;
                            break;
                        case HitboxType.Other:
                            color = Global.Settings.ShowOtherBox? Color.Black : Color.Transparent;
                            break;
                        default:
                            color = defaultColor;
                            break;
                    }

                    World.Draw.DrawSolidPolygon(vertices, vertexCount, color);
                    break;
                }
            }
        }

        #endregion
    }
}