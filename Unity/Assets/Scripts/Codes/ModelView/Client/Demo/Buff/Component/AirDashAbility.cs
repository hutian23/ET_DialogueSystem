namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class AirDashAbility : Entity, IAwake, IDestroy
    {
        public int dashCount;
        public int maxDashCount;
    }
}