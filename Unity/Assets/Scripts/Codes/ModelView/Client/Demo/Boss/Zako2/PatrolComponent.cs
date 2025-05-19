namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class PatrolComponent : Entity, IAwake, IDestroy, IGizmosUpdate
    {
        public float minX;
        public float maxX;
    }
}