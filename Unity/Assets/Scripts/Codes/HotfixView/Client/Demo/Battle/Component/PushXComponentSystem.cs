namespace ET.Client
{
    [FriendOf(typeof(PushXComponent))]
    public static class PushXComponentSystem
    {
        public class PushBackComponentAwakeSystem : AwakeSystem<PushXComponent,int, float, float>
        {
            protected override void Awake(PushXComponent self,int direction, float vel, float friction)
            {
                self.direction = direction;
                self.startVel = vel;
                self.curVel = vel;
                self.friction = friction;
                self.token = new ETCancellationToken();
                self.PushXCor().Coroutine();
            }
        }

        public class PushBackComponentDestroySystem : DestroySystem<PushXComponent>
        {
            protected override void Destroy(PushXComponent self)
            {
                self.direction = 0;
                self.startVel = 0;
                self.curVel = 0;
                self.friction = 0;
                self.token.Cancel();
            }
        }

        private static async ETTask PushXCor(this PushXComponent self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            b2Body b2Body = unit.GetComponent<b2Body>();
            
            while (self.curVel > 0f)
            {
                await bbTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;

                float dv = self.friction * (1 / 60f);
                self.curVel -= dv;
                b2Body.SetVelocityX(self.direction * self.curVel);
            }
            
            b2Body.SetVelocityX(0);
            self.Dispose();
        }
    }
}