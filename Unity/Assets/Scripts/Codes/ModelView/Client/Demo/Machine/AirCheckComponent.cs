using Box2DSharp.Dynamics;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class AirCheckComponent : Entity, IAwake<Fixture>, IDestroy
    {
        public Fixture checkBox;
        public long timer;
        public bool inAir;
    }
}