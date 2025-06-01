namespace ET.Client
{
    public static class BattleSceneManagerSystem
    {
        public class BattleSceneManagerAwakeSystem : AwakeSystem<BattleSceneManager>
        {
            protected override void Awake(BattleSceneManager self)
            {
                BattleSceneManager.Instance = self;
            }
        }
        
        public class BattleSceneManagerDestroySystem : DestroySystem<BattleSceneManager>
        {
            protected override void Destroy(BattleSceneManager self)
            {
                BattleSceneManager.Instance = null;
            }
        }
    }
}