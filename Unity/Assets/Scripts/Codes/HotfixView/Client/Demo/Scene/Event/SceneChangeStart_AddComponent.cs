using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(BBTimerManager))]
    public class SceneChangeStart_AddComponent : AEvent<EventType.SceneChangeStart>
    {
        protected override async ETTask Run(Scene scene, EventType.SceneChangeStart args)
        {
            Scene currentScene = scene.CurrentScene();
            // 加载场景资源
            await ResourcesComponent.Instance.LoadBundleAsync($"{currentScene.Name}.unity3d");
            // 切换到map场景
            await SceneManager.LoadSceneAsync(currentScene.Name);
            
            //管理逻辑帧
            currentScene.AddComponent<BBTimerManager>();
            //管理物理帧
            currentScene.AddComponent<b2WorldManager>();
            //管理输入
            currentScene.AddComponent<BBInputComponent>();
            //注册SceneTimer
            currentScene.AddComponent<CameraManager>();
            currentScene.AddComponent<EnemyManager>();
            currentScene.AddComponent<BallManager>();
        }
    }
}