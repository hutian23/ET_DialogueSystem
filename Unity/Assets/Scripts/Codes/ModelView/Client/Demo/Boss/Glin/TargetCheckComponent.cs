using Box2DSharp.Dynamics;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class TargetCheckComponent : Entity, IAwake, IDestroy, IPostStep
    {
        public bool InRange;
        public long UnitId;
        public Fixture Fixture;
    }
}