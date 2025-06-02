namespace ET.Client
{
    public static class TacticalTimeBuffSystem
    {
        public class TacticalTimeBuffAwakeSystem : AwakeSystem<TacticalTimeBuff, int, int>
        {
            protected override void Awake(TacticalTimeBuff self, int lastFrame, int hertz)
            {
                self.lastFrame = lastFrame;
                self.cnt = lastFrame;
                self.Hertz = hertz;
            }
        }
        
        [FriendOf(typeof(BBTimerManager))]
        public class TacticalTimeBuffDestroySystem : DestroySystem<TacticalTimeBuff>
        {
            protected override void Destroy(TacticalTimeBuff self)
            {
                self.lastFrame = 0;
                self.cnt = 0;
                self.Hertz = 0;
                
                int count = BBTimerManager.Instance.instanceIds.Count;
                while (count-- > 0)
                {
                    long instanceId = BBTimerManager.Instance.instanceIds.Dequeue();
                    BBTimerManager.Instance.instanceIds.Enqueue(instanceId);

                    if (Root.Instance.Get(instanceId) is not BBTimerComponent bbTimer || bbTimer.IsDisposed) continue;
                    
                    EventSystem.Instance.Invoke(new HertzChangeCallback(){instanceId = bbTimer.GetParent<Unit>().InstanceId, hertz = 60});
                }
            }
        }

        [FriendOf(typeof(BBTimerManager))]
        public class TacticalTimeBuffFrameUpdateSystem : FrameUpdateSystem<TacticalTimeBuff>
        {
            protected override void FrameUpdate(TacticalTimeBuff self)
            {
                if (self.cnt-- <= 0)
                {
                    self.Dispose();
                    return;
                }
                
                int count = BBTimerManager.Instance.instanceIds.Count;
                while (count -- > 0)
                {
                    long instanceId = BBTimerManager.Instance.instanceIds.Dequeue();
                    BBTimerManager.Instance.instanceIds.Enqueue(instanceId);

                    if (Root.Instance.Get(instanceId) is not BBTimerComponent bbTimer || bbTimer.IsDisposed) continue;

                    // 玩家不受魔女时间影响
                    Unit unit = bbTimer.GetParent<Unit>();
                    if (unit.GetComponent<PlayerManager>() != null) continue;
                    
                    // 抛出 TimeScale修改事件
                    EventSystem.Instance.Invoke(new HertzChangeCallback(){instanceId = bbTimer.GetParent<Unit>().InstanceId, hertz = self.Hertz});
                }
            }
        }
    }
}