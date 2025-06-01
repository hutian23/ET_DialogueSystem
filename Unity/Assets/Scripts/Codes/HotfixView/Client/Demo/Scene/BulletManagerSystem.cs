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
        
        public class BulletManagerLoadSystem : LoadSystem<BulletManager>
        {
            protected override void Load(BulletManager self)
            {
                ListComponent<Entity> removeList = ListComponent<Entity>.Create();
                removeList.AddRange(self.Children.Values);
                removeList.ForEach(entity => entity.Dispose());
                removeList.Dispose();
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