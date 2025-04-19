namespace ET.Client
{
    [FriendOf(typeof(GroundDashAbility))]
    public static class GroundDashAbilitySystem
    {
        public class GroundDashAbilityAwakeSystem : AwakeSystem<GroundDashAbility>
        {
            protected override void Awake(GroundDashAbility self)
            {
                self.cancelToken = new ETCancellationToken();
                self.ChargeCor().Coroutine();
            }
        }

        public class GroundDashAbilityDestroySystem : DestroySystem<GroundDashAbility>
        {
            protected override void Destroy(GroundDashAbility self)
            {
                self.cancelToken.Cancel();
            }
        }
        
        private static async ETTask ChargeCor(this GroundDashAbility self)
        {
            Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();

            while (true)
            {
                await bbTimer.WaitFrameAsync(self.cancelToken);
                if (self.cancelToken.IsCancel())
                {
                    break;
                }
                
                //1. 如果当前dash次数不为maxDash，开始充能
                if (self.dashCount < self.maxDashCount && buffManager.GetComponent<GroundDashRecharge>() != null)
                {
                    buffManager.AddComponent<GroundDashRecharge,int>(self.chargeFrame, true);
                }
            }
        }
    }
}