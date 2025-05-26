using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // 创建GameObject
            string bundleName = $"zako3.unity3d";
            await ResourcesComponent.Instance.LoadBundleAsync(bundleName);
            GameObject prefab = ResourcesComponent.Instance.GetAsset(bundleName, "Zako3") as GameObject;
            GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);

            // 创建unit
            Unit enemy = BulletManager.Instance.AddChild<Unit, int>(1001);
            enemy.AddComponent<GameObjectComponent>().GameObject = go;
            enemy.AddComponent<BBParser>();
            go.GetComponent<BBScript>().instanceId = enemy.InstanceId;

            // 初始位置
            b2Body b2Body = b2WorldManager.Instance.GetBody(enemy.InstanceId);
            b2Body.SetPosition(new Vector2(0, 0));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}