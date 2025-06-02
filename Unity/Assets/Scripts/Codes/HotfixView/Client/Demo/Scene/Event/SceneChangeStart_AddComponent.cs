using ET.EventType;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(BBTimerManager))]
    public class SceneChangeStart_AddComponent : AEvent<SceneChangeStart>
    {
        protected override async ETTask Run(Scene scene, SceneChangeStart args)
        {
            Scene currentScene = scene.CurrentScene();
            // 加载场景资源
            await ResourcesComponent.Instance.LoadBundleAsync($"{currentScene.Name}.unity3d");
            // 切换到map场景
            await SceneManager.LoadSceneAsync(currentScene.Name);
            
            currentScene.AddComponent<BattleSceneManager>();
            
            // 运行时生成的 Bullet、Enemy等unit全部挂载BulletManager下，热重载时统一销毁
            currentScene.AddComponent<BulletManager>();
            currentScene.AddComponent<EffectManager>();
            
            // 热重载时销毁PoolObject，对Prefab进行更新后热重载销毁旧的实例
            currentScene.AddComponent<PoolManager>();
            
            currentScene.AddComponent<BBInputManager>();            
            currentScene.AddComponent<CameraManager>();
            
            // 逻辑帧
            currentScene.AddComponent<BBTimerManager>();
            // 物理帧
            currentScene.AddComponent<b2WorldManager>();
        }
    }
}