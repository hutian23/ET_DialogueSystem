namespace ET.Client
{
    public static class PoolManagerSystem
    {
        public class PoolManagerLoadSystem : LoadSystem<PoolManager>
        {
            protected override void Load(PoolManager self)
            {
                GameObjectPoolHelper.Init();
            }
        }
        
        public class PoolManagerDestroySystem : DestroySystem<PoolManager>
        {
            protected override void Destroy(PoolManager self)
            {
                GameObjectPoolHelper.Init();
            }
        }
    }
}