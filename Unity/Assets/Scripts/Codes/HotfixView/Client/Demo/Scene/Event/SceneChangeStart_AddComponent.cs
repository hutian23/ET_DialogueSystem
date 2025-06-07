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
        }
    }
}