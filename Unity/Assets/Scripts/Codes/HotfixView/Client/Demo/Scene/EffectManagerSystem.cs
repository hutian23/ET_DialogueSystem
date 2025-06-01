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
        
        public class EffectManagerLoadSystem : LoadSystem<EffectManager>
        {
            protected override void Load(EffectManager self)
            {
                ListComponent<Entity> removeList = ListComponent<Entity>.Create();
                removeList.AddRange(self.Children.Values);
                removeList.ForEach(entity => entity.Dispose());
                removeList.Dispose();
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