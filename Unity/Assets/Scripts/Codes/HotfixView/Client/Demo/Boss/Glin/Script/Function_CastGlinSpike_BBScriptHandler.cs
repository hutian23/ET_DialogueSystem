using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    public class Function_CastGlinSpike_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CastGlinSpike";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 创建 bullet unit
            Unit caster = parser.GetParent<Unit>();
            Unit bullet = BulletManager.Instance.AddChild<Unit, int>(1001);
            GameObject go = GameObjectPoolHelper.GetObjectFromPool("GlinSpike");
            bullet.AddComponent<GameObjectComponent>().GameObject = go;
            bullet.AddComponent<BBParser>();

            //2. bullet 初始位置
            b2Body bodyA = b2WorldManager.Instance.GetBody(caster.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(bullet.InstanceId);
            
            bodyB.SetPosition(bodyA.GetPosition() + new Vector2(0, -0.4f));
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}