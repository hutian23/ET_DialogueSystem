namespace ET.Client
{
    [FriendOf(typeof(OnFireBuff))]
    public static class OnFireBuffSystem
    {
        public class OnFireBuffAwakeSystem : AwakeSystem<OnFireBuff, int, int, int>
        {
            protected override void Awake(OnFireBuff self, int interval, int totalFrame, int damage)
            {
                self.interval = interval;
                self.totalFrame = totalFrame;
                self.curFrame = totalFrame;
                self.damage = damage;
                self.token = new ETCancellationToken();
                self.OnFireCor().Coroutine();
            }
        }

        public class OnFireBuffDestroySystem : DestroySystem<OnFireBuff>
        {
            protected override void Destroy(OnFireBuff self)
            {
                self.interval = 0;
                self.totalFrame = 0;
                self.curFrame = 0;
                self.damage = 0;
                self.token.Cancel();
            }
        }
        
        private static async ETTask OnFireCor(this OnFireBuff self)
        {
            Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            HPAbility ability = buffManager.GetComponent<HPAbility>();    
            
            while (self.curFrame >= self.interval)
            {
                await bbTimer.WaitAsync(self.interval, self.token);
                if (self.token.IsCancel()) return;

                self.curFrame -= self.interval;

                int curHP = ability.GetHP();
                ability.SetHP(curHP - self.damage);
                
                Log.Warning($"OnFire! Current HP: {curHP} Damage: {self.damage}");
            }
            
            self.Dispose();
        }
    }
}