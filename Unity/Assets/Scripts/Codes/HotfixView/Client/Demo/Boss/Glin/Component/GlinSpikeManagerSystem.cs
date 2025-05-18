namespace ET.Client
{
    [FriendOf(typeof(GlinSpikeManager))]
    public static class GlinSpikeManagerSystem
    {
        public class GlinSpikeManagerDestroySystem : DestroySystem<GlinSpikeManager>
        {
            protected override void Destroy(GlinSpikeManager self)
            {
                self.offset = 0f;
                self.waitFrame = 0;
                self.spawnCount = 0;
                self.spawnIndexSet.Clear();
                self.token.Cancel();
            }
        }

        public static void StartSpawnCor(this GlinSpikeManager self, float offset, int waitFrame, int spawnCount)
        {
            self.offset = offset;
            self.waitFrame = waitFrame;
            self.spawnCount = spawnCount;
            self.spawnIndexSet.Clear();
            self.token = new ETCancellationToken();
            self.SpawnCor().Coroutine();
        }
        
        private static async ETTask SpawnCor(this GlinSpikeManager self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            while (self.spawnCount-- > 0)
            {
                await bbTimer.WaitAsync(self.waitFrame, self.token);
                if (self.token.IsCancel()) return;
            }
        }
    }
}