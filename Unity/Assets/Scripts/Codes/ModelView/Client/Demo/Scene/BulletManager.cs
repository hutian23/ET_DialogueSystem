namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class BulletManager : Entity, IAwake, IDestroy
    {
        [StaticField]
        public static BulletManager Instance;
    }
}