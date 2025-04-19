namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class GroundDashRecharge : Entity, IAwake<int>, IDestroy
    {
        public int counter;
        public ETCancellationToken token;
    }
}