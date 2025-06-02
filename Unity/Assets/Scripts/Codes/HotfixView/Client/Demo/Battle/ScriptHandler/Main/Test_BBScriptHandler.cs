using ET.Event;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // // 创建GameObject
            // string bundleName = $"zako3.unity3d";
            // await ResourcesComponent.Instance.LoadBundleAsync(bundleName);
            // GameObject prefab = ResourcesComponent.Instance.GetAsset(bundleName, "Zako3") as GameObject;
            // GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
            //
            // // 创建unit
            // Unit enemy = BulletManager.Instance.AddChild<Unit, int>(1001);
            // enemy.AddComponent<GameObjectComponent>().GameObject = go;
            // enemy.AddComponent<BBParser>();
            // go.GetComponent<BBScript>().instanceId = enemy.InstanceId;
            //
            // // 初始位置
            // b2Body b2Body = b2WorldManager.Instance.GetBody(enemy.InstanceId);
            // b2Body.SetPosition(new Vector2(0, 0));

            // HitComponent hit = parser.GetComponent<HitComponent>();
            // CollisionBuffer buffer = hit.GetBuffer();
            //
            // b2Box boxA = Root.Instance.Get(buffer.instanceIdA) as b2Box;
            // b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
            //
            // b2Body bodyA = boxA.GetParent<b2Body>();
            // b2Body bodyB = boxB.GetParent<b2Body>();
            // Unit unitA = Root.Instance.Get(bodyA.unitId) as Unit;
            // Unit unitB = Root.Instance.Get(bodyB.unitId) as Unit;
            //
            // GameObject goA = unitA.GetComponent<GameObjectComponent>().GameObject;
            // GameObject goB = unitB.GetComponent<GameObjectComponent>().GameObject;
            //
            // Log.Warning(goA.name + "  " + goB.name);

            // parser.GetParent<Unit>().GetComponent<BuffManager>().AddComponent<IcyFreezeBuff, int, long>(300, parser.GetParent<Unit>().InstanceId);

            parser.GetParent<Unit>().GetComponent<BuffManager>().AddComponent<TacticalTimeBuff, int, int>(100, 10, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}