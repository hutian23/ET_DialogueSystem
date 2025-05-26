using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(BBTimerManager))]
    public class SceneChangeStart_AddComponent : AEvent<ET.EventType.SceneChangeStart>
    {
        protected override async ETTask Run(Scene scene, ET.EventType.SceneChangeStart args)
        {
            Scene currentScene = scene.CurrentScene();
            // 加载场景资源
            await ResourcesComponent.Instance.LoadBundleAsync($"{currentScene.Name}.unity3d");
            // 切换到map场景
            await SceneManager.LoadSceneAsync(currentScene.Name);
            
            // 逻辑帧
            currentScene.AddComponent<BBTimerManager>();
            // 物理帧
            currentScene.AddComponent<b2WorldManager>();
            // 输入
            currentScene.AddComponent<BBInputComponent>();
            // 相机
            currentScene.AddComponent<CameraManager>();
        }
    }
}