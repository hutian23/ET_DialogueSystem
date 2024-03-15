namespace ET.Client
{
    [ChildOf(typeof(BehaviorBufferComponent))]
    public class TriggerCheck: Entity, IAwake, IDestroy
    {
        public uint targetID;
        public long lastedFrame;
    }
}