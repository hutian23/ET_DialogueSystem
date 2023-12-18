namespace ET.Client
{
    // [ComponentOf(typeof (Unit))]
    [ComponentOf]
    public class TODAIComponent: Entity, IAwake, IDestroy, ILoad, IUpdate
    {
        public ETCancellationToken Token;
        public AIBehaviorConfig config;
        public long AITimer;

        public int order;
    }
}