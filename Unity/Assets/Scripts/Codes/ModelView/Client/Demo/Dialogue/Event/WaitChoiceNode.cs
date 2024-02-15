namespace ET.Client
{
    public struct WaitChoiceNode: IWaitType
    {
        public uint next;
        public int Error { get; set; }
    }

    public struct WaitNextNode: IWaitType
    {
        public int Error { get; set; }
    }

    public struct AfterNodeExecuted
    {
        public long ID;
        public DialogueComponent component;
    }
}