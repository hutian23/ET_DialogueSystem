using ET.Event;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(b2GameManager))]
    public static class b2GameManagerSystem
    {
        public class b2WorldManagerAwakeSystem : AwakeSystem<b2GameManager>
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
            self.B2World?.Dispose();
            self.B2World = new b2World(self.Game);
            EventSystem.Instance.PublishAsync(self.DomainScene(), new AfterB2WorldCreated() { B2World = self.B2World }).Coroutine();
        }
        
        public class b2WorldManagerFixedUpdateSystem : FixedUpdateSystem<b2GameManager>
        {
            protected override void FixedUpdate(b2GameManager self)
            {
                self.B2World.Step();
                self.SyncTrans();
            }
        }

        private static void SyncTrans(this b2GameManager self)
        {
            foreach (Entity child in self.Children.Values)
            {
                b2Body body = child as b2Body;
                body.SyncUnitTransform();
            }
        }

        public class b2WorldManagerDestroySystem : DestroySystem<b2GameManager>
        {
            protected override void Destroy(b2GameManager self)
            {
                b2GameManager.Instance = null;
                self.Game = null;
                self.B2World?.Dispose();
            }
        }
    }
}