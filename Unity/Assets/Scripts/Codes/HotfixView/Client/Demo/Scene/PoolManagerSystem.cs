namespace ET.Client
{
    public static class PoolManagerSystem
    {
        public class PoolManagerDestroySystem : DestroySystem<PoolManager>
        {
            protected override void Destroy(PoolManager self)
            {
                GameObjectPoolHelper.Init();
            }
        }
    }
}