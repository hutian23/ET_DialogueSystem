using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof (ScriptParser))]
    public class AfterSceneChange_CreatePlayerView: AEvent<CreatePlayerView>
    {
        protected override async ETTask Run(Scene scene, CreatePlayerView args)
        {
            Unit player = TODUnitHelper.GetPlayer(scene.ClientScene());

            //1. 加载AB
            await ResourcesComponent.Instance.LoadBundleAsync($"{player.Config.ABName}.unity3d");
            GameObject prefab = (GameObject)ResourcesComponent.Instance.GetAsset($"{player.Config.ABName}.unity3d", $"{player.Config.Name}");
            GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);

            //2. 以下组件 切换场景时全部销毁
            player.AddComponent<ObjectWait>();
            player.AddComponent<GameObjectComponent>().GameObject = go;

            //3. Timeline
            TimelineComponent timelineComponent = player.AddComponent<TimelineComponent>();
            timelineComponent.AddComponent<BBTimerComponent>(); // 战斗相关的计时器(因为和角色行为逻辑关联性强，作为timeline的组件)
            timelineComponent.AddComponent<BBParser>();
            timelineComponent.AddComponent<TimelineEventManager>();
            timelineComponent.AddComponent<HitboxComponent>();
            timelineComponent.AddComponent<SkillBuffer>();
            timelineComponent.AddComponent<InputWait>();

            //4. timeline相关的资源，需要在创建player时加载
        }
    }
}