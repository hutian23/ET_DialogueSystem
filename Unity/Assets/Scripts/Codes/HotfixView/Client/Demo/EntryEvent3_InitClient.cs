namespace ET.Client
{
    [Event(SceneType.Process)]
    public class EntryEvent3_InitClient: AEvent<EventType.EntryEvent3>
    {
        protected override async ETTask Run(Scene scene, EventType.EntryEvent3 args)
        {
            // 加载配置
            Root.Instance.Scene.AddComponent<ResourcesComponent>();
            Root.Instance.Scene.AddComponent<GlobalComponent>();

            await ResourcesComponent.Instance.LoadBundleAsync("unit.unity3d");

            Scene clientScene = await SceneFactory.CreateClientScene(1, "Game");
            clientScene.AddComponent<Storage>();
            clientScene.AddComponent<Input>();
            clientScene.AddComponent<TODEventSystem>();
            clientScene.AddComponent<OperaComponent>();
            clientScene.AddComponent<ScriptDispatcherComponent>();
            // clientScene.AddComponent<DialogueDispatcherComponent>();

            Unit player = TODUnitFactory.CreatePlayer(clientScene);
            player.AddComponent<NumericComponent>();
            // player.AddComponent<DialogueStorageManager>();

            await Storage.Instance.SaveStorage(0, player);
            //反序列化存档
            Unit loadUnit = await Storage.Instance.LoadStorage(0);
            TODUnitHelper.AddPlayer(clientScene, loadUnit);

            await EventSystem.Instance.PublishAsync(clientScene, new EventType.AppStartInitFinish());
        }
    }
}