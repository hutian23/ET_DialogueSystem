using ET.Event;
using UnityEngine;

namespace ET.Client
{
    public static class b2GameManagerSystem
    {
        public class b2WorldManagerAwakeSystem: AwakeSystem<b2GameManager>
        {
            protected override void Awake(b2GameManager self)
            {
                b2GameManager.Instance = self;
                self.Game = Camera.main.GetComponent<b2Game>();
                self.B2World = new b2World(self.Game);
                EventSystem.Instance.PublishAsync(self.DomainScene(), new AfterB2WorldCreate() { B2World = self.B2World }).Coroutine();
            }
        }

        public class b2WorldManagerLoadSystem: LoadSystem<b2GameManager>
        {
            protected override void Load(b2GameManager self)
            {
                self.B2World?.Dispose();
                self.B2World = new b2World(self.Game);
                EventSystem.Instance.PublishAsync(self.DomainScene(), new AfterB2WorldCreate() { B2World = self.B2World }).Coroutine();
            }
        }

        public class b2WorldManagerFixedUpdateSystem: FixedUpdateSystem<b2GameManager>
        {
            protected override void FixedUpdate(b2GameManager self)
            {
                self.B2World.Step();
            }
        }

        public class b2WorldManagerDestroySystem: DestroySystem<b2GameManager>
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