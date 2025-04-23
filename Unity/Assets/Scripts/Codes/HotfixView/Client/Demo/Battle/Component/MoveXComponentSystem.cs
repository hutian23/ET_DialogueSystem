namespace ET.Client
{
    [FriendOf(typeof(MoveXComponent))]
    public static class MoveXComponentSystem
    {
        public class MoveXComponentAwakeSystem : AwakeSystem<MoveXComponent, float>
        {
            protected override void Awake(MoveXComponent self, float vel)
            {
                self.MoveX = vel;
                self.token = new ETCancellationToken();
                self.MoveCor().Coroutine();
            }
        }

        public class MoveXComponentDestroySystem : DestroySystem<MoveXComponent>
        {
            protected override void Destroy(MoveXComponent self)
            {
                self.MoveX = 0;
                self.token.Cancel();
            }
        }

        private static async ETTask MoveCor(this MoveXComponent self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            
            while (true)
            {
                await bbTimer.WaitFrameAsync(self.token);
                if(self.token.IsCancel()) return;

                b2Unit.SetVelocityX(self.MoveX);
            }
        }
    }
}