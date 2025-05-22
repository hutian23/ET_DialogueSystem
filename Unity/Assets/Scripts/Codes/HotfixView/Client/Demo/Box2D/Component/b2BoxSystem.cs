using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(b2Box))]
    public static class b2BoxSystem
    {
        [FriendOf(typeof(b2Body))]
        public class b2BoxAwakeSystem : AwakeSystem<b2Box, FixtureDef>
        {
            protected override void Awake(b2Box self, FixtureDef def)
            {
                b2Body b2Body = self.GetParent<b2Body>();
                self.fixture = b2Body.body.CreateFixture(def);

                FixtureData data = (FixtureData)def.UserData;
                self.fixtureName = data.Name;
            }
        }
        
        [FriendOf(typeof(b2Body))]
        public class b2BoxDestroySystem : DestroySystem<b2Box>
        {
            protected override void Destroy(b2Box self)
            {
                b2Body b2Body = self.GetParent<b2Body>();
                b2Body.body.DestroyFixture(self.fixture);
                self.fixture = null;

                self.fixtureName = string.Empty;
            }
        }

        public static Fixture GetFixture(this b2Box self)
        {
            return self.fixture;
        }
    }
}