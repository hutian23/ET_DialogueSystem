namespace ET.Client
{
    public static class HotReloadManagerSystem
    {
        public class HotReloadManagerAwakeSystem : AwakeSystem<HotReloadManager>
        {
            protected override void Awake(HotReloadManager self)
            {
                
            }
        }
        
        public class HotReloadManagerDestroySystem : DestroySystem<HotReloadManager>
        {
            protected override void Destroy(HotReloadManager self)
            {
                self.BBScriptQueue.Clear();
            }
        }
    }
}