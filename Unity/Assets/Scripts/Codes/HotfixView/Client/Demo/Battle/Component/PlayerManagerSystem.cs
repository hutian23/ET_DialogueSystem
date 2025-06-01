namespace ET.Client
{
    public static class PlayerManagerSystem
    {
        public class PlayerManagerAwakeSystem : AwakeSystem<PlayerManager>
        {
            protected override void Awake(PlayerManager self)
            {
                PlayerManager.Instance = self;
            }
        }
        
        public class PlayerManagerDestroySystem : DestroySystem<PlayerManager>
        {
            protected override void Destroy(PlayerManager self)
            {
                PlayerManager.Instance = null;
            }
        }
    }
}