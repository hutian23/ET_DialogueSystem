namespace ET.Client
{
    [FriendOf(typeof(JumpMoveXComponent))]
    public static class JumpMoveXComponentSystem
    {
        public class JumpMoveXAwakeSystem : AwakeSystem<JumpMoveXComponent, float>
        {
            protected override void Awake(JumpMoveXComponent self, float vel)
            {
                self.vel = vel;
                self.token = new ETCancellationToken();
                self.MoveCor().Coroutine();
            }
        }

        public class JumpMoveXDestroySystem : DestroySystem<JumpMoveXComponent>
        {
            protected override void Destroy(JumpMoveXComponent self)
            {
                self.vel = 0;
                self.token.Cancel();
            }
        }

        private static async ETTask MoveCor(this JumpMoveXComponent self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            InputWait inputWait = unit.GetComponent<InputWait>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            while (true)
            {
                await bbTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;
                
                //输入左右相关的指令才会生效水平移动的效果
                FlipState flip = 0;
                if (inputWait.IsPressing(BBOperaType.RIGHT) || 
                    inputWait.IsPressing(BBOperaType.DOWNRIGHT) ||
                    inputWait.IsPressing(BBOperaType.UPRIGHT))
                {
                    flip = FlipState.Right;
                }
                else if (inputWait.IsPressing(BBOperaType.LEFT) ||
                         inputWait.IsPressing(BBOperaType.DOWNLEFT) ||
                         inputWait.IsPressing(BBOperaType.UPLEFT))
                {
                    flip = FlipState.Left;
                }
                
                b2Body.SetVelocityX((int)flip * self.vel * b2Body.GetFlip());
            }
        }
    }
}