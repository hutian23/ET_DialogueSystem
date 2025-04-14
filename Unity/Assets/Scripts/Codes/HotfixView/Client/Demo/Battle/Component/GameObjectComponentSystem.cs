namespace ET.Client
{
    public static class GameObjectComponentSystem
    {
        public class GameObjectComponentDestroySystem : DestroySystem<GameObjectComponent>
        {
            protected override void Destroy(GameObjectComponent self)
            {
                //1. 池化管理
                if (self.GameObject.GetComponent<PoolObject>() != null)
                {
                    GameObjectPoolHelper.ReturnObjectToPool(self.GameObject);
                    return;
                }
                
                //2. 非对象池管理，直接销毁
                UnityEngine.Object.Destroy(self.GameObject);
                self.GameObject = null;
            }
        }
    }
}