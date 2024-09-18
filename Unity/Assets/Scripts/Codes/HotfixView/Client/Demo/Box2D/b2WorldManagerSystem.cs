using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;

namespace ET.Client
{
    public static class b2WorldManagerSystem
    {
        public class b2WorldManagerAwakeSystem: AwakeSystem<b2GameManager>
        {
            protected override void Awake(b2GameManager self)
            {
                b2GameManager.Instance = self;
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