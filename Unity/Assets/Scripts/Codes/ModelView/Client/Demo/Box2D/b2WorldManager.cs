namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class b2WorldManager: Entity, IAwake, IDestroy
    {
        [StaticField]
        public static b2WorldManager Instance;
    }
}