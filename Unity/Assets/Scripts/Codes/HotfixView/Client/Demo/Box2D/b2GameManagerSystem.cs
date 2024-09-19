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
            }
        }
    }
}