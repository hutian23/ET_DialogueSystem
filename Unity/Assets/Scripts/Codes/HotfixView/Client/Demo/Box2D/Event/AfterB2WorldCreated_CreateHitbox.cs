using Box2DSharp.Dynamics;
using ET.Event;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(TimelineManager))]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(b2GameManager))]    
    //这个事件用于建立Unit和b2Wolrd中刚体的映射
    public class AfterB2WorldCreated_CreateHitbox : AEvent<AfterB2WorldCreated>
    {
        protected override async ETTask Run(Scene scene, AfterB2WorldCreated args)
        {
            foreach (long instanceId in TimelineManager.Instance.instanceIds)
            {
                TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
                World world = args.B2World.World;

                //创建刚体
                var bodyDef = new BodyDef()
                {
                    BodyType = BodyType.DynamicBody,
                    Position = new Vector2(0, 4),
                    GravityScale = 0f,
                    LinearDamping = 0f,
                    AngularDamping = 0f,
                    AllowSleep = true,
                    FixedRotation = true
                };
                var body = world.CreateBody(bodyDef);

                b2Body b2Body = b2GameManager.Instance.AddChild<b2Body>();
                b2Body.body = body;
                b2Body.unitId = timelineComponent.GetParent<Unit>().InstanceId;
                b2GameManager.Instance.BodyDict.TryAdd(b2Body.unitId, b2Body);
            }

            await ETTask.CompletedTask;
        }
    }
}