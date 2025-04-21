namespace ET.Client
{
    [FriendOf(typeof(TimeFrozeComponent))]
    public static class TimeFrozeComponentSystem
    {
        public class TimeFrozeComponentAwakeSystem : AwakeSystem<TimeFrozeComponent, int, int, long>
        {
            protected override void Awake(TimeFrozeComponent self, int hertz, int lastFrame, long instanceId)
            {
                self.Hertz = hertz;
                self.LastFrame = lastFrame;
                self.abilityId = instanceId;
                
                self.token = new ETCancellationToken();
                self.TimeFrozeCor().Coroutine();
            }
        }

        public class TimeFrozeComponentDestroySystem : DestroySystem<TimeFrozeComponent>
        {
            protected override void Destroy(TimeFrozeComponent self)
            {
                self.Hertz = 0;
                self.LastFrame = 0;
                self.abilityId = 0;
                
                self.token.Cancel();
            }
        }

        private static async ETTask TimeFrozeCor(this TimeFrozeComponent self)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            HertzAbility ability = Root.Instance.Get(self.abilityId) as HertzAbility;
            
            while (self.LastFrame-- > 0)
            {
                ability.SetHertz(self.Hertz);
                await sceneTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) break;
            }
            ability.SetHertz(60);
            
            self.Dispose();
        }
    }
}