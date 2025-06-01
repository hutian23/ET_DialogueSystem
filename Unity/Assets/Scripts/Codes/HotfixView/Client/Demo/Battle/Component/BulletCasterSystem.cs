namespace ET.Client
{
    public static class BulletCasterSystem
    {
        public class BulletCasterAwakeSystem : AwakeSystem<BulletCaster, long, int>
        {
            protected override void Awake(BulletCaster self, long _instanceId, int filterType)
            {
                self._instanceId = _instanceId;
                self.filterType = filterType;
                self.GetParent<b2Body>().RegistFilter(filterType);
            }
        }
        
        public class BulletCasterDestroySystem : DestroySystem<BulletCaster>
        {
            protected override void Destroy(BulletCaster self)
            {
                self._instanceId = 0;
                self.GetParent<b2Body>().RemoveFilter(self.filterType);
                self.filterType = 0;
            }
        }
    }
}