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
}