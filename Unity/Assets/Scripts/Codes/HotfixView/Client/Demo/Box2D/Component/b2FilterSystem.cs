namespace ET.Client
{
    public static class b2FilterSystem
    {
        public class b2FilterDestroySystem : DestroySystem<b2Filter>
        {
            protected override void Destroy(b2Filter self)
            {
                self.type = 0;
            }
        }
        
        public class b2FilterAwakeSystem : AwakeSystem<b2Filter, int>
        {
            protected override void Awake(b2Filter self, int filterType)
            {
                self.type = filterType;
            }
        }
    }
}