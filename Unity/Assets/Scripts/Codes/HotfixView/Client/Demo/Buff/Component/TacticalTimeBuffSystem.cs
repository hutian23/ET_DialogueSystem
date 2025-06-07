namespace ET.Client
{
    [FriendOf(typeof(BBTimerManager))]
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
        public class TacticalTimeBuffFrameUpdateSystem : FrameUpdateSystem<TacticalTimeBuff>
        {
            protected override void FrameUpdate(TacticalTimeBuff self)
            {
                if (self.cnt-- <= 0)
                {
                    self.Dispose();
                    return;
                }
                
                // 子弹时间之后生成的Unit同样受影响
                // TODO 需要单独开一个子弹时间专用的时间轴
                self.Freeze(self.Hertz);
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
                self.Freeze(60);
            }
        }

        private static void Freeze(this TacticalTimeBuff _, int hertz)
        {
            int count = BBTimerManager.Instance.instanceIds.Count;
            while (count-- > 0)
            {
                long instanceId = BBTimerManager.Instance.instanceIds.Dequeue();
                BBTimerManager.Instance.instanceIds.Enqueue(instanceId);
                
                // Unit已经销毁
                if (Root.Instance.Get(instanceId) is not BBTimerComponent bbTimer || bbTimer.IsDisposed) continue;

                // 玩家和玩家生成的 子弹 特效 不受魔女时间影响
                Unit unit = bbTimer.GetParent<Unit>();
                if (unit.GetUnitType() is UnitType.Player) continue;

                EventSystem.Instance.Invoke(new HertzChangeCallback() { instanceId = bbTimer.GetParent<Unit>().InstanceId, hertz = hertz });   
            }
        }
    }
}