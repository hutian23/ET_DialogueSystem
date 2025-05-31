namespace ET.Client
{
    [FriendOf(typeof(GroundDashRecharge))]
    public static class GroundDashRechargeSystem
    {
        public class GroundDashRechargeAwakeSystem : AwakeSystem<GroundDashRecharge, int>
        {
            protected override void Awake(GroundDashRecharge self, int filterType)
            {
                self.counter = filterType;
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
            Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            GroundDashAbility gd = unit.GetComponent<BuffManager>().GetComponent<GroundDashAbility>();

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
            gd.SetDashCount(gd.GetMaxDashCount());
            
            //3. 移除自己
            self.Dispose();
        }
    }
}