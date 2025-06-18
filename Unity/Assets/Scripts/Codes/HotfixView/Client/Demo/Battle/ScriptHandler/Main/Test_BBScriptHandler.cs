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
            // 创建GameObject
            string bundleName = $"zako3.unity3d";
            await ResourcesComponent.Instance.LoadBundleAsync(bundleName);
            GameObject prefab = ResourcesComponent.Instance.GetAsset(bundleName, "Zako3") as GameObject;
            GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
            
            // 创建unit
            Unit enemy = BattleSceneManager.Instance.AddChild<Unit, int>(1001);
            enemy.AddComponent<GameObjectComponent>().GameObject = go;
            enemy.AddComponent<BBParser>();
            go.GetComponent<BBScript>().instanceId = enemy.InstanceId;
            
            // Log.Warning("1");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}