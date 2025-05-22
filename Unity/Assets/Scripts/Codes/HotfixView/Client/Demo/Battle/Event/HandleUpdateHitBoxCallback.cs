using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(b2WorldManager))]
    [FriendOf(typeof(b2Box))]    //HitboxTrack的回调
    public class HandleUpdateHitBoxCallback : AInvokeHandler<UpdateHitboxCallback>
    {
        public override void Handle(UpdateHitboxCallback args)
        {
            // 查询组件
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            if (unit == null || unit.InstanceId == 0) return;

            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            //1. 销毁旧的夹具
            b2Body.ClearFixtures(FixtureType.Hitbox);

            //2. 更新hitbox
            foreach (BoxInfo info in args.Keyframe.boxInfos)
            {
                PolygonShape shape = new();
                shape.SetAsBox(info.size.x / 2, info.size.y / 2, new Vector2(info.center.x * b2Body.GetFlip(), info.center.y), 0f);
                FixtureDef fixtureDef = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0f,
                    UserData = new FixtureData()
                    {
                        InstanceId = b2Body.InstanceId,
                        Name = info.boxName,
                        Type = FixtureType.Hitbox,
                        LayerType = LayerType.Unit,
                        IsTrigger = info.hitboxType is not HitboxType.Squash,
                        UserData = info,
                        TriggerEnterId = TriggerEnterType.HandleCallback,
                        TriggerStayId = TriggerStayType.HandleCallback,
                        TriggerExitId = TriggerExitType.HandleCallback,
                        CollisionEnterId = CollisionEnterType.HandleCallback,
                        CollisionStayId = CollisionStayType.HandleCallback,
                        CollisionExitId = CollisionExitType.HandleCallback,
                    }
                };
                b2Body.CreateFixture(fixtureDef);
            }

            // foreach (BoxInfo info in args.Keyframe.boxInfos)
            // {
            //     b2Box b2Box = b2Body.AddChild<b2Box>();
            //
            //     #region 判定框初始化
            //     // 层级关系
            //     b2Box.LayerType = info.layerType;
            //     b2Box.TagType = info.tagType;
            //     
            //     // 触发器
            //     b2Box.IsTrigger = info.hitboxType is not HitboxType.Squash;
            //     
            //     b2Box.Name = info.boxName;
            //     b2Box.center = info.center.ToVector2();
            //     b2Box.size = info.size.ToVector2();
            //     b2Box.HitboxType = info.hitboxType;
            //     
            //     //碰撞回调
            //     b2Box.TriggerEnterId = TriggerEnterType.HandleCallback;
            //     b2Box.TriggerStayId = TriggerStayType.HandleCallback;
            //     b2Box.TriggerExitId = TriggerExitType.HandleCallback;
            //     b2Box.CollisionEnterId = CollisionEnterType.HandleCallback;
            //     b2Box.CollisionStayId = CollisionStayType.HandleCallback;
            //     b2Box.CollisionExitId = CollisionExitType.HandleCallback;
            //     #endregion
            //
            //     #region 生成夹具
            //     // 夹具形状
            //     PolygonShape shape = new();
            //     shape.SetAsBox(b2Box.size.X / 2f, b2Box.size.Y / 2f, b2Box.center * new Vector2(b2Body.GetFlip(), 1), 0f);
            //     
            //     // 传入b2Box.instanceId
            //     FixtureDef fixtureDef = new() { Shape = shape, Density = 1.0f, Friction = 0f, UserData = b2Box.InstanceId };
            //     b2Box.fixture = b2Body.CreateFixture(fixtureDef);
            //     
            //     #endregion
            // }
        }
    }
}