using ET.EventType;

namespace ET.Client
{
    public static class BBSceneChangeHelper
    {
        public static async ETTask SceneChangeTo(Scene clientScene, string sceneName)
        {
            //1. 删除之前的currentScene.创建新的
            CurrentScenesComponent currentScenesComponent = clientScene.GetComponent<CurrentScenesComponent>();
            currentScenesComponent.Scene?.Dispose();

            //2.切换场景 玩家的角色控制器 和视图相关组件要独立处理(因为都挂载clientScene上)
            Scene currentScene = SceneFactory.CreateCurrentScene(IdGenerater.Instance.GenerateId(), clientScene.Zone, sceneName, currentScenesComponent);
            currentScene.AddComponent<UnitComponent>();
            
            //3. 加载场景
            await EventSystem.Instance.PublishAsync(clientScene, new SceneChangeStart());
            //4. 场景初始化
            await EventSystem.Instance.PublishAsync(clientScene, new BattleSceneInit());
            //5. 渲染层创建玩家,并且初始化
            await EventSystem.Instance.PublishAsync(clientScene, new CreatePlayerView());   
            //6. 切换场景完成
            await EventSystem.Instance.PublishAsync(clientScene, new SceneChangeFinish());
        }
    }
}