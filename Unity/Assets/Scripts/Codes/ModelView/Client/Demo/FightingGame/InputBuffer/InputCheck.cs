namespace ET.Client
{
    [ChildOf(typeof(BehaviorBufferComponent))]
    public class InputCheck: Entity, IAwake, IDestroy
    {
        public uint targetID;
        public long LastedFrame;
        //输入检测协程
        public string inputChecker;
    }
}