namespace ET.Client
{
    [FriendOf(typeof(InvincibleAbility))]
    public static class InvincibleAbilitySystem
    {
        public class InvincibleAbilityAwakeSystem : AwakeSystem<InvincibleAbility, int>
        {
            protected override void Awake(InvincibleAbility self, int waitFrame)
            {
                self.waitFrame = waitFrame;
                self.token = new ETCancellationToken();
                self.InvincibleCor().Coroutine();
            }
        }

        public class InvincibleAbilityDestroySystem : DestroySystem<InvincibleAbility>
        {
            protected override void Destroy(InvincibleAbility self)
            {
                self.waitFrame = 0;
                self.token.Cancel();
            }
        }

        private static async ETTask InvincibleCor(this InvincibleAbility self)
        {
            Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            // b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            // 注册ContactFilter规则
            // body.RegistFilter(FilterType.InvincibleFilter);
            await bbTimer.WaitAsync(self.waitFrame, self.token);
            // body.RemoveFilter(FilterType.InvincibleFilter);
            
            if (self.token.IsCancel()) return;
            self.Dispose();
        }
    }
}