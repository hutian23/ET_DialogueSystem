namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class GroundDashAbility : Entity, IAwake, IDestroy
    {
        public int dashCount;
        public int maxDashCount;
        public int chargeFrame;
        public ETCancellationToken cancelToken;
    }
}