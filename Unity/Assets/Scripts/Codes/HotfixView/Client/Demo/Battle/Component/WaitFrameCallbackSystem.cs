namespace ET.Client
{
    [FriendOf(typeof(WaitFrameCallback))]
    [FriendOf(typeof(BBParser))]
    public static class WaitFrameCallbackSystem
    {
        public class WaitFrameCallbackDestroySystem : DestroySystem<WaitFrameCallback>
        {
            protected override void Destroy(WaitFrameCallback self)
            {
                self.waitFrame = 0;
                self.functionIndex = 0;
                self.token.Cancel();
            }
        }

        public class WaitFrameCallbackAwakeSystem : AwakeSystem<WaitFrameCallback, int, int>
        {
            protected override void Awake(WaitFrameCallback self, int waitFrame, int functionIndex)
            {
                self.waitFrame = waitFrame;
                self.functionIndex = functionIndex;
                self.token = new ETCancellationToken();
                self.WaitFrameCor().Coroutine();
            }
        }

        private static async ETTask WaitFrameCor(this WaitFrameCallback self)
        {
            BBParser parser = self.GetParent<BBParser>();
            BBTimerComponent bbTimer = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();

            await bbTimer.WaitAsync(self.waitFrame, self.token);

            parser.Invoke(self.functionIndex, parser.CancellationToken).Coroutine();
        }
    }
}