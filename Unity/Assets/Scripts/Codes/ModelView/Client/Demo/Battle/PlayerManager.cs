namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class PlayerManager : Entity, IAwake, IDestroy, IGizmosUpdate
    {
        [StaticField]
        public static PlayerManager Instance;
    }
}