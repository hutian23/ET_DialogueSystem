using System.Numerics;
using Box2DSharp.Collision.Collider;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(GroundCollisionComponent))]
    public class Function_GroundCollisionVelocity_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GroundCollisionVelocity";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            b2Body b2body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            GroundCollisionComponent component = parser.GetComponent<GroundCollisionComponent>();
            
            CollisionInfo info = component.info;
            info.Contact.GetWorldManifold(out WorldManifold worldManifold);
            
            Vector2 normal = worldManifold.Normal; // 法向量
            Vector2 inVector = b2body.GetVelocity(); // 入射向量
            Vector2 outVector2 = Box2DHelper.GetReflection(inVector, normal); // 入射向量
            
            //1. 更新速度方向
            b2body.SetVelocity(outVector2);
            //2. 更新朝向
            b2body.SetRotation(-outVector2.Vector2ToRadians());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}