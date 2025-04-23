namespace ET.Client
{
    [FriendOf(typeof(IdleAnimComponent))]
    public static class IdleAnimComponentSystem
    {
        public class IdleAnimAwakeSystem : AwakeSystem<IdleAnimComponent, int, string>
        {
            protected override void Awake(IdleAnimComponent self, int counter,string behaviorName)
            {
                self.counter = counter;
                self.behaviorName = behaviorName;
                self.token = new ETCancellationToken();

                self.IdleAnimCor().Coroutine();
            }
        }

        public class IdleAnimDestroySystem : DestroySystem<IdleAnimComponent>
        {
            protected override void Destroy(IdleAnimComponent self)
            {
                self.counter = 0;
                self.behaviorName = string.Empty;
                self.token.Cancel();
            }
        }

        private static async ETTask IdleAnimCor(this IdleAnimComponent self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            
            while (self.counter -- > 0)
            {
                await bbTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;
            }
            
            machine.Reload(self.behaviorName);
        }
    }
}