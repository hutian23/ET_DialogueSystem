namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class TimelineComponent: Entity, IAwake, IDestroy
    {
    }

    //回调，调用后退出事件
    public struct CancelBehaviorCallback
    {
        public long instanceId;
    }
    
    public struct BeforeBehaviorReload
    {
        public long instanceId;
        public int behaviorOrder;
    }

    public struct AfterBehaviorReload
    {
        public long instanceId;
        public int behaviorOrder;
    }

    public struct AfterTimelineEvaluated
    {
        public long instanceId;
        public int targetFrame;
    }
}