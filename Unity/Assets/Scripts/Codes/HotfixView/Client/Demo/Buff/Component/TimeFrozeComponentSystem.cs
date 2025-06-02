namespace ET.Client
{
    [FriendOf(typeof(TimeFrozeComponent))]
    public static class TimeFrozeComponentSystem
    {
        public class TimeFrozeComponentFrameUpdateSystem : FrameUpdateSystem<TimeFrozeComponent>
        {
            protected override void FrameUpdate(TimeFrozeComponent self)
            {
                Unit unit = Root.Instance.Get(self.unitId) as Unit;
                BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
                
                if (self.LastFrame-- <= 0)
                {
                    self.Dispose();
                    return;
                }

                bbTimer.SetHertz(self.Hertz);
            }
        }
        
        public class TimeFrozeComponentDestroySystem : DestroySystem<TimeFrozeComponent>
        {
            protected override void Destroy(TimeFrozeComponent self)
            {
                self.Hertz = 0;
                self.LastFrame = 0;
                self.unitId = 0;
            }
        }
    }
}