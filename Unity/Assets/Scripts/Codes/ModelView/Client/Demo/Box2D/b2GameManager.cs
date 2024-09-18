using Box2DSharp.Dynamics;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class b2GameManager: Entity, IAwake, IDestroy
    {
        [StaticField]
        public static b2GameManager Instance;

        public World world;
    }
}