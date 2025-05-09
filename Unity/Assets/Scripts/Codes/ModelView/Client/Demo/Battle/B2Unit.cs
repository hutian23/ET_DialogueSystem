namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class B2Unit : Entity, IAwake, IDestroy
    {
        public long unitId;
    }
}