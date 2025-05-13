namespace ET.Client
{
    public static class EffectManagerSystem
    {
        public class EffectManagerAwakeSystem : AwakeSystem<EffectManager>
        {
            protected override void Awake(EffectManager self)
            {
                EffectManager.Instance = self;
            }
        }
        
        public class EffectManagerDestroySystem : DestroySystem<EffectManager>
        {
            protected override void Destroy(EffectManager self)
            {
                EffectManager.Instance = null;
            }
        }
    }   
}