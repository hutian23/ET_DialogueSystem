namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class ZakoChaseComponent : Entity, IAwake, IDestroy, IGizmosUpdate
    {
        public float distance;
        public float velocity;
    }
}