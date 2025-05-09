namespace ET.Client
{
    [FriendOf(typeof(GravityCheckAbility))]
    public static class GravityCheckAbilitySystem
    {
        public class GravityCheckComponentAwakeSystem : AwakeSystem<GravityCheckAbility>
        {
            protected override void Awake(GravityCheckAbility self)
            {
                self.token = new ETCancellationToken();
                self.GravityCheckCor().Coroutine();
            }
        }

        public class GravityCheckComponentDestroySystem : DestroySystem<GravityCheckAbility>
        {
            protected override void Destroy(GravityCheckAbility self)
            {
                self.token.Cancel();
                self.gravity = 0f;
                self.maxGravity = 0f;
                self.maxFall = 0f;
            }
        }

        private static async ETTask GravityCheckCor(this GravityCheckAbility self)
        {
            Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
            b2Body b2Body = unit.GetComponent<b2Body>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            while (true)
            {
                await bbTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;
                
                //1. 小于0，不受重力影响
                if (self.gravity <= 0f) continue;

                //2. y轴方向当前帧速度改变量
                float dv = -(1 / 60f) * self.gravity;

                //3. 约束最大下落速度
                float curV = b2Body.GetVelocity().Y + dv;
                float maxV = -self.maxFall; 
                b2Body.SetVelocityY(curV < maxV ? maxV : curV);
            }
        }
    }
}