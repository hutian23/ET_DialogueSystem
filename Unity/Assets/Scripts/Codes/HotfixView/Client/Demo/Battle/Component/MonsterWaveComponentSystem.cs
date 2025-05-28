namespace ET.Client
{
    [FriendOf(typeof(MonsterWaveComponent))]
    public static class MonsterWaveComponentSystem
    {
        public class MonsterWaveComponentDestroySystem : DestroySystem<MonsterWaveComponent>
        {
            protected override void Destroy(MonsterWaveComponent self)
            {
                self.monsterQueue.Clear();
            }
        }

        public static void RegistEnemy(this MonsterWaveComponent self, MonsterWaveFlag flag)
        {
            self.monsterQueue.Enqueue(flag.InstanceId);
        }

        public static async ETTask<Status> WaitClear(this MonsterWaveComponent self, ETCancellationToken token)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();

            while (true)
            {
                int count = self.monsterQueue.Count;
                while (count -- > 0)
                {
                    long instanceId = self.monsterQueue.Dequeue();
                    // 怪物已被消灭
                    if (Root.Instance.Get(instanceId) is not MonsterWaveFlag flag || flag.IsDisposed) continue;
                    self.monsterQueue.Enqueue(instanceId);
                }
                
                // 全部怪物被消灭
                if(self.monsterQueue.Count == 0) return Status.Success;
                
                await sceneTimer.WaitFrameAsync(token);
                if (token.IsCancel()) return Status.Failed;
            }
        }
    }
}