namespace ET.Client
{
    public static class EndBattleManagerSystem
    {
        public class EndBattleManagerAwakeSystem : AwakeSystem<EndBattleManager>
        {
            protected override void Awake(EndBattleManager self)
            {
                EndBattleManager.Instance = self;
            }
        }
        
        public class EndBattleManagerDestroySystem : DestroySystem<EndBattleManager>
        {
            protected override void Destroy(EndBattleManager self)
            {
                EndBattleManager.Instance = null;
            }
        }

        public static void EndBattleCallback(this EndBattleManager self)
        {
            EventSystem.Instance.Invoke(new BBActionCallback(){instanceId = self.InstanceId});
        }
    }
}