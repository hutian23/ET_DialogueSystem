namespace ET.Client
{
    [FriendOf(typeof(AirMoveXComponent))]
    public static class AirMoveXComponentSystem
    {
        public class AirMoveComponentAwakeSystem : AwakeSystem<AirMoveXComponent, float>
        {
            protected override void Awake(AirMoveXComponent self, float filterType)
            {
                self.vel = filterType;
                self.token = new ETCancellationToken();
                self.MoveCor().Coroutine();
            }
        }
        
        public class AirMoveComponentDestroySystem : DestroySystem<AirMoveXComponent>
        {
            protected override void Destroy(AirMoveXComponent self)
            {
                self.vel = 0;
                self.token.Cancel();
            }
        }

        private static async ETTask MoveCor(this AirMoveXComponent self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            InputComponent inputComponent = unit.GetComponent<InputComponent>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            while (true)
            {
                await bbTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;

                //输入左右相关的指令才会生效水平移动的效果
                bool direction = inputComponent.IsPressing(BBOperaType.MIDDLE) || inputComponent.IsPressing(BBOperaType.UP) || inputComponent.IsPressing(BBOperaType.DOWN);
                b2Body.SetVelocityX(direction? 0 : self.vel);
            }
        }
    }
}