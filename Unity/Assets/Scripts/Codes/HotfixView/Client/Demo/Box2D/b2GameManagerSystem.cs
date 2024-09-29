using ET.Event;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (b2GameManager))]
    [FriendOf(typeof (b2Body))]
    public static class b2GameManagerSystem
    {
        public class b2WorldManagerAwakeSystem: AwakeSystem<b2GameManager>
        {
            protected override void Awake(b2GameManager self)
            {
                b2GameManager.Instance = self;
                self.Game = Camera.main.GetComponent<b2Game>();
                self.Reload();
            }
        }

        public static void Reload(this b2GameManager self)
        {
            //create new b2World
            self.B2World?.Dispose();
            self.B2World = new b2World(self.Game);

            //dispose b2body
            foreach (var kv in self.BodyDict)
            {
                kv.Value.Dispose();
            }

            self.BodyDict.Clear();

            //create hitbox
            EventSystem.Instance.PublishAsync(self.DomainScene(), new AfterB2WorldCreated() { B2World = self.B2World }).Coroutine();
        }

        public class b2WorldManagerFixedUpdateSystem: FixedUpdateSystem<b2GameManager>
        {
            protected override void FixedUpdate(b2GameManager self)
            {
                self.UpdateVelocity();
                self.B2World.Step();
                self.SyncTrans();
            }
        }

        public class b2WorldManagerDestroySystem: DestroySystem<b2GameManager>
        {
            protected override void Destroy(b2GameManager self)
            {
                b2GameManager.Instance = null;
                self.Game = null;
                self.B2World?.Dispose();
                self.BodyDict.Clear();
            }
        }

        private static void UpdateVelocity(this b2GameManager self)
        {
            // foreach (var child in self.Children.Values)
            // {
            //     b2Body B2body = child as b2Body;
            //     B2body.body.SetLinearVelocity(new Vector2(0, 10));
            //     B2body.body.IsAwake = true;
            // }
        }

        private static void SyncTrans(this b2GameManager self)
        {
            foreach (Entity child in self.Children.Values)
            {
                b2Body body = child as b2Body;
                body.SyncUnitTransform();
            }
        }

        public static b2Body GetBody(this b2GameManager self, long unitId)
        {
            if (self.BodyDict.TryGetValue(unitId, out b2Body body))
            {
                return body;
            }

            return null;
        }
    }
}