namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BulletManager : Entity, IAwake, ILoad, IDestroy
    {
        [StaticField]
        public static BulletManager Instance;
    }
}