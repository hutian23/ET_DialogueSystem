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
                
                //2. 场景中的GameObject，保留，热重载时重载其挂载的BBScript
                if (self.GameObject.GetComponent<SceneObject>() != null)
                {
                    return;
                }
                
                //3. 直接销毁
                UnityEngine.Object.Destroy(self.GameObject);
                self.GameObject = null;
            }
        }
    }
}