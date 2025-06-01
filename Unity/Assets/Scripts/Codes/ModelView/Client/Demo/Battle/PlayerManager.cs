namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class PlayerManager : Entity, IAwake, IDestroy, ILoadCached
    {
        [StaticField]
        public static PlayerManager Instance;
    }
}