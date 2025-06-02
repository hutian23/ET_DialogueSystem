namespace ET.Client
{
    [FriendOf(typeof(TimeFrozeComponent))]
    public static class TimeFrozeComponentSystem
    {
        public class TimeFrozeComponentFrameUpdateSystem : FrameUpdateSystem<TimeFrozeComponent>
        {
            protected override void FrameUpdate(TimeFrozeComponent self)
            {
                if (self.LastFrame-- <= 0)
                {
                    self.Dispose();
                }
            }
        }
    }
}