namespace ET.Client
{
    public static class MarkerEventSystem
    {
        public class MarkerEventAwakeSystem : AwakeSystem<MarkerEvent, string, int>
        {
            protected override void Awake(MarkerEvent self, string markerName, int functionIndex)
            {
                self.markerName = markerName;
                self.functionIndex = functionIndex;
            }
        }
        
        public class MarkerEventDestroySystem : DestroySystem<MarkerEvent>
        {
            protected override void Destroy(MarkerEvent self)
            {
                self.markerName = string.Empty;
                self.functionIndex = -1;
            }
        }
    }
}