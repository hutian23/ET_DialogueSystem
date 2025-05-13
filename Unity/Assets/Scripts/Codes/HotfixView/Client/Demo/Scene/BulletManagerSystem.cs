namespace ET.Client
{
    public static class BulletManagerSystem
    {
        public class BulletManagerAwakeSystem : AwakeSystem<BulletManager>
        {
            protected override void Awake(BulletManager self)
            {
                BulletManager.Instance = self;
            }
        }
        
        public class BulletManagerDestroySystem : DestroySystem<BulletManager>
        {
            protected override void Destroy(BulletManager self)
            {
                BulletManager.Instance = null;
            }
        }
    }
}