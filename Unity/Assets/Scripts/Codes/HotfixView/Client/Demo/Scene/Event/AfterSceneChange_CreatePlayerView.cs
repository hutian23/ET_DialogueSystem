using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AfterSceneChange_CreatePlayerView: AEvent<CreatePlayerView>
    {
        protected override async ETTask Run(Scene scene, CreatePlayerView args)
        {
            Unit player = BBUnitHelper.GetPlayer(scene.ClientScene());

            //1. 加载AB
            await ResourcesComponent.Instance.LoadBundleAsync($"{player.Config.ABName}.unity3d");
            
            GameObject prefab = (GameObject)ResourcesComponent.Instance.GetAsset($"{player.Config.ABName}.unity3d", $"{player.Config.Name}");
            GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
            
            //2. 渲染层，传入playerId
            player.AddComponent<GameObjectComponent>().GameObject = go;
            BBScript bbScript = go.GetComponent<BBScript>();
            bbScript.instanceId = player.InstanceId;
            
            //3. 启动player行为机
            player.AddComponent<BBParser>();
        }
    }
}