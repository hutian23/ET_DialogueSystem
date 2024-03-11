namespace ET.Client
{
    [FriendOf(typeof(BBBehaviorBufferComponent))]
    public static class BBBehaviorBufferComponentSystem
    {
        public class BBBehaviorBufferComponentAwakeSystem : AwakeSystem<BBBehaviorBufferComponent>
        {
            protected override void Awake(BBBehaviorBufferComponent self)
            {
                self.Init();
            }
        }
        
        public class BBBehaviorBufferComponentUpdateSystem : UpdateSystem<BBBehaviorBufferComponent>
        {
            protected override void Update(BBBehaviorBufferComponent self)
            {
                
            }
        }
        
        private static void Init(this BBBehaviorBufferComponent self)
        {
            self.bufferDict.Clear();
            self.bufferIds.Clear();
            self.bufferOutQueue.Clear();
        }
    }
}