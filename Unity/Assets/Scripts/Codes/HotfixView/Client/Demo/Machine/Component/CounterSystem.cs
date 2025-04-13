namespace ET.Client
{
    [FriendOf(typeof(Counter))]
    public static class CounterSystem
    {
        public class CounterAwakeSystem : AwakeSystem<Counter, int>
        {
            protected override void Awake(Counter self, int frame)
            {
                self.totalFrame = frame;
                self.curFrame = frame;
                self.Token = new ETCancellationToken();
                self.CounterCor().Coroutine();
            }
        }

        public class CoroutineDestroySystem : DestroySystem<Counter>
        {
            protected override void Destroy(Counter self)
            {
                self.totalFrame = 0;
                self.curFrame = 0;
                self.Token.Cancel();
            }
        }
        
        private static async ETTask CounterCor(this Counter self)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();

            //1. 定时器任务执行中
            while (self.curFrame-- > 0)
            {
                await sceneTimer.WaitFrameAsync(self.Token);
                if (self.Token.IsCancel())
                {
                    return;
                }
            }
            
            //2. 执行完毕，销毁定时器组件
            self.Dispose();
        }

        public static int GetCounter(this Counter self)
        {
            return self.curFrame;
        }
    }
}