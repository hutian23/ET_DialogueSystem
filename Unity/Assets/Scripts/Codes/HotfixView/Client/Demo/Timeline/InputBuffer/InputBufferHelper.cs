namespace ET.Client
{
    [FriendOf(typeof (CancelManager))]
    [FriendOf(typeof (SkillInfo))]
    public static class InputBufferHelper
    {
        public static bool CancelCheck(SkillInfo skillInfo)
        {
            CancelManager cancelManager = skillInfo.GetParent<SkillBuffer>().GetParent<TimelineComponent>().GetComponent<CancelManager>();
            //cancelWindow does not opened
            if (cancelManager.infoId == 0) return true;
            return cancelManager.CancelableDict[skillInfo.behaviorOrder];
        }

        //避免组件互相耦合
        public static void Cancel(this CancelManager self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref self.Timer);

            self.infoId = 0;
            self.Timer = 0;
            self.GcOptions.Clear();
            self.CancelableDict.Clear();
        }

        public static void Start(this CancelManager self, SkillInfo info)
        {
            //保存配置组件的instanceId
            self.infoId = info.InstanceId;

            //注册配置组件Order --- 组件映射
            SkillBuffer buffer = info.GetParent<SkillBuffer>();
            foreach (Entity child in buffer.Children.Values)
            {
                SkillInfo skillInfo = child as SkillInfo;
                self.CancelableDict.Add(skillInfo.behaviorOrder, false);
            }

            //启动定时器
            BBTimerComponent bbTimer = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            bbTimer.NewFrameTimer(BBTimerInvokeType.CancelWindowTimer, self);
        }
    }
}