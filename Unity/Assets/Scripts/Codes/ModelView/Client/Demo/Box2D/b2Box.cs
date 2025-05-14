using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Client
{
    [ChildOf(typeof(b2Body))]
    public class b2Box : Entity, IAwake<FixtureDef>, IDestroy
    {
        public Fixture fixture;
        public string fixtureName;
        public FixtureType fixtureType;
    }
}