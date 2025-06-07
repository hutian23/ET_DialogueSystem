namespace ET.Client
{
    public static class HotReloadManagerSystem
    {
        public class HotReloadManagerAwakeSystem : AwakeSystem<HotReloadManager>
        {
            protected override void Awake(HotReloadManager self)
            {
                HotReloadManager.Instance = self;
                EventSystem.Instance.Invoke(new HotReloadInitCallback());
            }
        }
        
        public class HotReloadManagerLoadSystem : LoadSystem<HotReloadManager>
        {
            protected override void Load(HotReloadManager self)
            {
                EventSystem.Instance.Invoke(new HotReloadCallBack());
            }
        }
        
        public class HotReloadManagerDestroySystem : DestroySystem<HotReloadManager>
        {
            protected override void Destroy(HotReloadManager self)
            {
                HotReloadManager.Instance = null;
                self.BBScriptQueue.Clear();
            }
        }
    }
}