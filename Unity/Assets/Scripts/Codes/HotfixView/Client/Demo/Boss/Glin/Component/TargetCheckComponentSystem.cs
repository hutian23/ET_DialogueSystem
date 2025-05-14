using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    public static class TargetCheckComponentSystem
    {
        public class TargetCheckComponentAwakeSystem : AwakeSystem<TargetCheckComponent>
        {
            protected override void Awake(TargetCheckComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
                
                //1. 创建夹具
                PolygonShape shape = new();
                shape.SetAsBox(2, 2, new Vector2(0f, 0f), 0f);
                FixtureDef def = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0f,
                    UserData = new FixtureData()
                    {
                        InstanceId = body.InstanceId,
                        Name = "TargetCheckBox",
                        Type = FixtureType.Default,
                        LayerMask = LayerType.Unit,
                        IsTrigger = true,
                        UserData = new BoxInfo(),
                        TriggerStayId = TriggerStayType.HandleCallback,
                        CollisionStayId = CollisionStayType.HandleCallback
                    }
                };
                
                //2. 
                self.UnitId = unit.InstanceId;
                self.Fixture = body.CreateFixture(def);
                self.InRange = false;
            }
        }
    }
}