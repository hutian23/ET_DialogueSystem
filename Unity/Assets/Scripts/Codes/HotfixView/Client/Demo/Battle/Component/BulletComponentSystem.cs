namespace ET.Client
{
    public static class BulletComponentSystem
    {
        public class BulletComponentDestroySystem : DestroySystem<BulletComponent>
        {
            protected override void Destroy(BulletComponent self)
            {
                GameObjectPoolHelper.ReturnObjectToPool(self.GameObject);
            }
        }
    }
}