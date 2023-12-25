namespace ET.Client
{
    [FriendOf(typeof (PunchBuffer))]
    public static class PunchBufferSystem
    {
        public static void ResetTimer(this PunchBuffer self, long timer)
        {
            TimerComponent.Instance.Remove(ref self.bufferTimer);
            self.bufferTimer = TimerComponent.Instance.NewOnceTimer(TimeInfo.Instance.ClientNow() + timer, TimerInvokeType.RemoveBufferTimer, self);
        }

        public class PunchBufferDestroySystem: DestroySystem<PunchBuffer>
        {
            protected override void Destroy(PunchBuffer self)
            {
                TimerComponent.Instance.Remove(ref self.bufferTimer);
            }
        }
    }
}