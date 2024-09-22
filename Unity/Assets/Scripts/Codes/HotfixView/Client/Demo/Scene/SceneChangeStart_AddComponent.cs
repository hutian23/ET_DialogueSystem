using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeStart_AddComponent: AEvent<EventType.SceneChangeStart>
    {
        protected override async ETTask Run(Scene scene, EventType.SceneChangeStart args)
        {
            Scene currentScene = scene.CurrentScene();

            // 加载场景资源
            await ResourcesComponent.Instance.LoadBundleAsync($"{currentScene.Name}.unity3d");
            // 切换到map场景
            await SceneManager.LoadSceneAsync(currentScene.Name);

            currentScene.AddComponent<GameManager>();
            //添加物理世界管理组件
            currentScene.AddComponent<b2GameManager>();
            //Timeline管理
            currentScene.AddComponent<TimelineManager>();
        }
    }
}