using ET.EventType;

namespace ET.Client
{
    public static class BBSceneChangeHelper
    {
        public static async ETTask SceneChangeTo(Scene clientScene, string sceneName)
        {
            //1. 删除之前的currentScene, 创建新的
            CurrentScenesComponent currentScenesComponent = clientScene.GetComponent<CurrentScenesComponent>();
            currentScenesComponent.Scene?.Dispose();
            SceneFactory.CreateCurrentScene(IdGenerater.Instance.GenerateId(), clientScene.Zone, sceneName, currentScenesComponent);
            
            //2. 加载场景
            await EventSystem.Instance.PublishAsync(clientScene, new SceneChangeStart());
            //3. 场景初始化
            await EventSystem.Instance.PublishAsync(clientScene, new BattleSceneInit());
            //5. 切换场景完成
            await EventSystem.Instance.PublishAsync(clientScene, new SceneChangeFinish());
        }
    }
}