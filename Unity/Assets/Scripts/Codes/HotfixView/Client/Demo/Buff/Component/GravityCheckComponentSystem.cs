namespace ET.Client
{
    public static class GravityCheckComponentSystem
    {
        [Invoke(BBTimerInvokeType.GravityCheckTimer)]
        [FriendOf(typeof(GravityCheckComponent))]
        public class GravityCheckTimer : BBTimer<GravityCheckComponent>
        {
            protected override void Run(GravityCheckComponent self)
            {
                Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
                B2Unit b2Unit = unit.GetComponent<B2Unit>();
                
                //1. 小于0，不受重力影响
                if (self.gravity <= 0f) return;

                //2. y轴方向当前帧速度改变量
                float dv = - (1 / 60f) * self.gravity;
                
                //3. 约束最大下落速度
                float curV = b2Unit.GetVelocity().Y + dv;
                float maxV = -self.maxFall;
                b2Unit.SetVelocityY(curV < maxV ? maxV : curV);
            }
        }

        public class GravityCheckComponentAwakeSystem : AwakeSystem<GravityCheckComponent>
        {
            protected override void Awake(GravityCheckComponent self)
            {
                BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
                self.timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.GravityCheckTimer, self);
            }
        }
        
        public class GravityCheckComponentDestroySystem : DestroySystem<GravityCheckComponent>
        {
            protected override void Destroy(GravityCheckComponent self)
            {
                BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
                postStepTimer.Remove(ref self.timer);
                
                self.gravity = 0f;
                self.maxGravity = 0f;
                self.maxFall = 0f;
            }
        }
    }
}