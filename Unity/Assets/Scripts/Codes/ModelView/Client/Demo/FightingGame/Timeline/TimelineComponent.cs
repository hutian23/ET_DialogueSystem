namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class TimelineComponent: Entity, IAwake, IDestroy
    {
    }

    public struct AfterBehaviorReload
    {
        public long instanceId;
        public int behaviorOrder;
    }
}