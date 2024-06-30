namespace ET.Client
{
    public struct WaitBlock: IWaitType
    {
        public int Error { get; set; }
    }

    public struct WaitCounterHit: IWaitType
    {
        public int Error { get; set; }
    }

    public struct WaitHit: IWaitType
    {
        public int Error { get; set; }
    }

    public struct WaitNextBehavior: IWaitType
    {
        public int Error { get; set; }
        public long order;
    }

    //TestManager 中断当前行为，预览下一个行为
    public struct WaitStopCurrentBehavior: IWaitType
    {
        public int Error { get; set; }
        public long order; // 传递给WaitNextBehavior
    }
}