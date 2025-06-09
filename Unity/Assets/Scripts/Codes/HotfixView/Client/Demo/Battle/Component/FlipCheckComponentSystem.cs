namespace ET.Client
{
    [FriendOf(typeof(FlipCheckComponent))]
    public static class FlipCheckComponentSystem
    {
        public class FlipCheckComponentAwakeSystem : AwakeSystem<FlipCheckComponent>
        {
            protected override void Awake(FlipCheckComponent self)
            {
                self.cancelToken = new ETCancellationToken();
                self.FlipCoroutine().Coroutine();
            }
        }

        public class FlipCheckComponentDestroySystem : DestroySystem<FlipCheckComponent>
        {
            protected override void Destroy(FlipCheckComponent self)
            {
                self.cancelToken.Cancel();
            }
        }

        private static async ETTask FlipCoroutine(this FlipCheckComponent self)
        {                       
            //1. 查询组件
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            InputComponent inputComponent = unit.GetComponent<InputComponent>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            while (true)
            {
                //2. 等待一帧
                await bbTimer.WaitFrameAsync(self.cancelToken);
                if (self.cancelToken.IsCancel()) return;   
                
                //3. 更新刚体朝向
                if (inputComponent.IsPressing(BBOperaType.LEFT) ||
                    inputComponent.IsPressing(BBOperaType.DOWNLEFT) ||
                    inputComponent.IsPressing(BBOperaType.UPLEFT))
                {
                    body.SetFlip(FlipState.Left);
                }
                else if (inputComponent.IsPressing(BBOperaType.RIGHT) ||
                         inputComponent.IsPressing(BBOperaType.DOWNRIGHT) ||
                         inputComponent.IsPressing(BBOperaType.UPRIGHT))
                {
                    body.SetFlip(FlipState.Right);
                }
            }
        }
    }
}