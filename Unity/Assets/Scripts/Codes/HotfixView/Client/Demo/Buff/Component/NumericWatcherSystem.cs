namespace ET.Client
{
    [FriendOf(typeof(NumericWatcher))]
    public static class NumericWatcherSystem
    {
        public class HPWatcherDestroySystem : DestroySystem<NumericWatcher>
        {
            protected override void Destroy(NumericWatcher self)
            {
                self._instanceId = 0;
                self.functionIndex = 0;
            }
        }

        public static string GenerateKey(this NumericWatcher self)
        {
            return string.Concat(self._instanceId, self.functionIndex);
        }
    }
}