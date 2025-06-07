namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class PlayerManager : Entity, IAwake, IDestroy
    {
        [StaticField]
        public static PlayerManager Instance;
    }
}