namespace ET.Client
{
    [FriendOf(typeof(TimeFrozeComponent))]
    public static class TimeFrozeComponentSystem
    {
        public class TimeFrozeComponentAwakeSystem : AwakeSystem<TimeFrozeComponent, int, int, long>
        {
            protected override void Awake(TimeFrozeComponent self, int hertz, int lastFrame, long unitId)
            {
                self.Hertz = hertz;
                self.LastFrame = lastFrame;
                self.cnt = 0;
                self.unitId = unitId;
                EventSystem.Instance.Invoke(new HertzChangeCallback(){instanceId = self.unitId, hertz = hertz});
            }
        }
        
        public class TimeFrozeComponentFrameUpdateSystem : FrameUpdateSystem<TimeFrozeComponent>
        {
            protected override void FrameUpdate(TimeFrozeComponent self)
            {
                if (self.cnt++ < self.LastFrame) return;
                self.Dispose();
            }
        }
        
        public class TimeFrozeComponentDestroySystem : DestroySystem<TimeFrozeComponent>
        {
            protected override void Destroy(TimeFrozeComponent self)
            {
                EventSystem.Instance.Invoke(new HertzChangeCallback(){instanceId = self.unitId, hertz = 60});
                self.Hertz = 0;
                self.LastFrame = 0;
                self.cnt = 0;
                self.unitId = 0;
            }
        }
    }
}