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
        
        public static Unit Get(this UnitComponent self, long id)
        {
            Unit unit = self.GetChild<Unit>(id);
            return unit;
        }

        public static void Remove(this UnitComponent self, long id)
        {
            Unit unit = self.GetChild<Unit>(id);
            unit?.Dispose();
        }
    }
}