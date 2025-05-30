using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Box))]
    public class HandleCreateB2BoxCallback : AInvokeHandler<CreateB2BoxCallback>
    {
        public override void Handle(CreateB2BoxCallback args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            if (unit == null || unit.IsDisposed) return;

            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            b2BoxDef boxDef = args.boxDef;

            //1. 创建组件,初始化数值
            b2Box b2Box = b2Body.AddBox(boxDef.Name);

            // 层级关系
            b2Box.LayerType = boxDef.layerType;
            b2Box.TagType = boxDef.TagType;

            // 触发器
            b2Box.IsTrigger = boxDef.IsTrigger;

            b2Box.Name = boxDef.Name;
            b2Box.Center = boxDef.Center;
            b2Box.Size = boxDef.Size;
            b2Box.HitboxType = boxDef.HitboxType;
            
            // 碰撞回调
            b2Box.TriggerEnterId = boxDef.TriggerEnterId;
            b2Box.TriggerStayId = boxDef.TriggerStayId;
            b2Box.TriggerExitId = boxDef.TriggerExitId;
            b2Box.CollisionEnterId = boxDef.CollisionEnterId;
            b2Box.CollisionStayId = boxDef.CollisionStayId;
            b2Box.CollisionExitId = boxDef.CollisionExitId;
            
            //2. 生成夹具
            PolygonShape shape = new();
            shape.SetAsBox(b2Box.Size.X / 2f, b2Box.Size.Y / 2f, b2Box.Center * new Vector2(b2Body.GetFlip(), 1), 0);

            //3. 传入b2box.instanceId
            FixtureDef fixtureDef = new() { Shape = shape, Density = 1.0f, Friction = 0f, UserData = b2Box.InstanceId };
            b2Box.fixture = b2Body.CreateFixture(fixtureDef);
            b2Box.fixtureDef = fixtureDef;
        }
    }
}