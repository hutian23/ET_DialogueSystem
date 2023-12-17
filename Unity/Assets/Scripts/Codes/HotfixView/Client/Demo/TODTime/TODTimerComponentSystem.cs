namespace ET.Client
{
    [FriendOf(typeof (TODTimerComponent))]
    public static class TODTimerComponentSystem
    {
        public static long GetId(this TODTimerComponent self)
        {
            return ++self.idGenerator;
        }

        public static long GetNow(this TODTimerComponent self)
        {
            return self.curFrame;
        }

        public static void Init(this TODTimerComponent self)
        {
            self.TimerId.Clear();
            self.timeOutTime.Clear();
            self.timeOutTimerIds.Clear();
            self.timerActions.Clear();

            self.timeScale = 1f;
            self.minFrame = long.MaxValue;
            self.curFrame = 0;
            self.deltaTimereminder = 0f;
        }
    }
}