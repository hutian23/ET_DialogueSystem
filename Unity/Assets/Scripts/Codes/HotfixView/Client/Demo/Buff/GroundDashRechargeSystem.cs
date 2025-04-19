namespace ET.Client
{
    [FriendOf(typeof(GroundDashRecharge))]
    [FriendOf(typeof(GroundDashAbility))]
    public static class GroundDashRechargeSystem
    {
        public class GroundDashRechargeAwakeSystem : AwakeSystem<GroundDashRecharge, int>
        {
            protected override void Awake(GroundDashRecharge self, int chargeFrame)
            {
                self.counter = chargeFrame;
                self.token = new ETCancellationToken();
                self.DashRechargeCor().Coroutine();
            }
        }

        public class GroundDashReChargeDestroySystem : DestroySystem<GroundDashRecharge>
        {
            protected override void Destroy(GroundDashRecharge self)
            {
                self.counter = 0;
                self.token.Cancel();
            }
        }

        private static async ETTask DashRechargeCor(this GroundDashRecharge self)
        {
            BuffManager buffManager = self.GetParent<BuffManager>();
            Unit unit = buffManager.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            //1. 等待
            while (self.counter-- >= 0)
            {
                await bbTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel())
                {
                    return;
                }
            }

            //2. 充能
            GroundDashAbility gd = buffManager.GetComponent<GroundDashAbility>();
            gd.dashCount = gd.maxDashCount;
            
            //3. 移除自己
            self.Dispose();
        }
    }
}