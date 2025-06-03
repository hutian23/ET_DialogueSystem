namespace ET.Client
{
    [FriendOf(typeof(BBAction))]
    public static class BBActionSystem
    {
        public class BBActionAwakeSystem : AwakeSystem<BBAction, long, int>
        {
            protected override void Awake(BBAction self, long _instanceId, int functionIndex)
            {
                self._instanceId = _instanceId;
                self.functionIndex = functionIndex;
            }
        }
        
        public class BBActionDestroySystem : DestroySystem<BBAction>
        {
            protected override void Destroy(BBAction self)
            {
                self._instanceId = 0;
                self.functionIndex = 0;
            }
        }
    }
}