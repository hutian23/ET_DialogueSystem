using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Box))]
    public class HandleUpdateHitBoxCallback : AInvokeHandler<UpdateHitboxCallback>
    {
        public override void Handle(UpdateHitboxCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null || timelineComponent.InstanceId == 0) return;
            
            //1. 销毁旧的夹具
            Unit unit = timelineComponent.GetParent<Unit>();
            if (unit == null || unit.InstanceId == 0) return;
            
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            b2Body.DestroyBoxes(Box2DHelper.HitboxMask);
            
            //2. 根据关键帧更新Hitbox
            foreach (BoxInfo info in args.Keyframe.boxInfos)
            {
                //3. 添加b2box组件
                b2Box b2Box = b2Body.AddBox(info.boxName);
                
                //4. b2Box初始化
                //层级关系
                b2Box.LayerType = info.layerType;
                b2Box.TagType = info.tagType;
                
                //触发器
                b2Box.IsTrigger = info.hitboxType is not HitboxType.Squash;
                
                b2Box.Name = info.boxName;
                b2Box.Center = info.center.ToVector2();
                b2Box.Size = info.size.ToVector2();
                b2Box.HitboxType = info.hitboxType;
                
                //碰撞回调
                b2Box.TriggerEnterId = TriggerEnterType.HandleCallback;
                b2Box.TriggerStayId = TriggerStayType.HandleCallback;
                b2Box.TriggerExitId = TriggerExitType.HandleCallback;
                b2Box.CollisionEnterId = CollisionEnterType.HandleCallback;
                b2Box.CollisionStayId = CollisionStayType.HandleCallback;
                b2Box.CollisionExitId = CollisionExitType.HandleCallback;
                
                //5. 生成夹具
                //夹具形状
                PolygonShape shape = new();
                shape.SetAsBox(b2Box.Size.X / 2f, b2Box.Size.Y / 2f, b2Box.Center * new Vector2(b2Body.GetFlip(), 1), 0);
                
                //传入b2Box.instanceId
                FixtureDef fixtureDef = new() { Shape = shape, Density = 1.0f, Friction = 0f, UserData = b2Box.InstanceId };
                b2Box.fixture = b2Body.CreateFixture(fixtureDef);
                b2Box.fixtureDef = fixtureDef;
            }
        }
    }
}