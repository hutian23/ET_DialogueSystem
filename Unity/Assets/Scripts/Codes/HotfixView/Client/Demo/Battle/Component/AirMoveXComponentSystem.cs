namespace ET.Client
{
    [FriendOf(typeof(AirMoveXComponent))]
    public static class AirMoveXComponentSystem
    {
        public class AirMoveComponentAwakeSystem : AwakeSystem<AirMoveXComponent, float>
        {
            protected override void Awake(AirMoveXComponent self, float vel)
            {
                self.vel = vel;
                self.inertiaEffect = true;
                self.token = new ETCancellationToken();
                self.MoveCor().Coroutine();
            }
        }
        
        public class AirMoveComponentDestroySystem : DestroySystem<AirMoveXComponent>
        {
            protected override void Destroy(AirMoveXComponent self)
            {
                self.vel = 0;
                self.inertiaEffect = false;
                self.token.Cancel();
            }
        }

        private static async ETTask MoveCor(this AirMoveXComponent self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            InputWait inputWait = unit.GetComponent<InputWait>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            while (true)
            {
                await bbTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;

                //输入左右相关的指令才会生效水平移动的效果
                bool direction = inputWait.IsPressing(BBOperaType.MIDDLE) || inputWait.IsPressing(BBOperaType.UP) || inputWait.IsPressing(BBOperaType.DOWN);
                
                //当前回中，则不会进行移动
                if (self.inertiaEffect && direction) return;
                self.inertiaEffect = false;

                b2Unit.SetVelocityX(direction? 0 : self.vel);
            }
        }
    }
}