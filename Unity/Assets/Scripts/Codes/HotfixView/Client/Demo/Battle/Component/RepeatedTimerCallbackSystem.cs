namespace ET.Client
{
    [FriendOf(typeof(RepeatedTimerCallback))]
    [FriendOf(typeof(BBParser))]
    public static class RepeatedTimerCallbackSystem
    {
        public class RepeatedTimerAwakeSystem : AwakeSystem<RepeatedTimerCallback, int, int>
        {
            protected override void Awake(RepeatedTimerCallback self, int waitFrame, int functionIndex)
            {
                self.waitFrame = waitFrame;
                self.functionIndex = functionIndex;
                self.token = new ETCancellationToken();
                self.RepeatedCor().Coroutine();
            }
        }

        public class RepeatedTimerDestroySystem : DestroySystem<RepeatedTimerCallback>
        {
            protected override void Destroy(RepeatedTimerCallback self)
            {
                self.waitFrame = 0;
                self.functionIndex = 0;
                self.token.Cancel();
            }
        }

        private static async ETTask RepeatedCor(this RepeatedTimerCallback self)
        {
            BBParser parser = self.GetParent<BBParser>();
            BBTimerComponent bbTimer = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();

            while (true)
            {
                await bbTimer.WaitAsync(self.waitFrame, self.token);
                if (self.token.IsCancel()) return;

                parser.Invoke(self.functionIndex, parser.CancellationToken).Coroutine();
            }
        }
    }
}