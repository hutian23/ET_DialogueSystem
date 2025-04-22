namespace ET.Client
{
    public static class LoopComponentSystem
    {
        public class LoopComponentDestroySystem : DestroySystem<LoopComponent>
        {
            protected override void Destroy(LoopComponent self)
            {
                self.triggerIndex = 0;
                self.startIndex = 0;
                self.endIndex = 0;
                self.token.Cancel();
            }
        }

        public static async ETTask TriggerCor(this LoopComponent self)
        {
            BBParser parser = self.GetParent<BBParser>();
            
            // string loopTrigger = parser.
            await ETTask.CompletedTask;
        }
    }
}