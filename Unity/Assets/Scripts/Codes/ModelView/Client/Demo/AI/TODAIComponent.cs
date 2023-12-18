namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class TODAIComponent: Entity, IAwake, IDestroy
    {
        public ETCancellationToken Token;
        public AIBehaviorConfig config;
        public long AITimer;

        public int order;
    }
}