using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class BattleSceneInit_AddComponent : AEvent<BattleSceneInit>
    {
        protected override async ETTask Run(Scene scene, BattleSceneInit args)
        {
            Scene currentScene = scene.CurrentScene();
            
            // 运行时生成的unit(包括Bullet VFX Enemy)全部挂载BattleSceneManager下，热重载时统一销毁
            currentScene.AddComponent<BattleSceneManager>();
            
            // 热重载时销毁PoolObject，对Prefab进行更新后热重载销毁旧的实例
            currentScene.AddComponent<PoolManager>();
            
            currentScene.AddComponent<BBInputManager>();            
            currentScene.AddComponent<CameraManager>();
            
            // 逻辑帧
            currentScene.AddComponent<BBTimerManager>();
            // 物理帧
            currentScene.AddComponent<b2WorldManager>();
            
            // 热重载管理器，场景中挂载了BBScript脚本的GameObject缓存在此单例中
            currentScene.AddComponent<HotReloadManager>();
            
            await ETTask.CompletedTask;
        }
    }
}