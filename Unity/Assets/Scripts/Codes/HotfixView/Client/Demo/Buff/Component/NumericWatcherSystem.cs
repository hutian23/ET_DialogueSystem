namespace ET.Client
{
    [FriendOf(typeof(NumericWatcher))]
    public static class NumericWatcherSystem
    {
        public class HPWatcherAwakeSystem : AwakeSystem<NumericWatcher, long, int, string>
        {
            protected override void Awake(NumericWatcher self, long _instanceId, int functionIndex, string watcherName)
            {
                self._instanceId = _instanceId;
                self.functionIndex = functionIndex;
                self.watcherName = watcherName;
            }
        }
        
        public class HPWatcherDestroySystem : DestroySystem<NumericWatcher>
        {
            protected override void Destroy(NumericWatcher self)
            {
                self._instanceId = 0;
                self.functionIndex = 0;
                self.watcherName = string.Empty;
            }
        }
    }
}