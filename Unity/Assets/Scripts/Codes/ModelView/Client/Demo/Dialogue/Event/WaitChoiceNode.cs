namespace ET.Client
{
    public struct WaitChoiceNode : IWaitType
    {
        public uint next;
        public int Error { get; set; }
    }
}