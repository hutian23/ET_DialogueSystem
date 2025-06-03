namespace ET.Client
{
    [FriendOf(typeof(MarkerEventManager))]
    public static class MarkerEventManagerSystem
    {
        public class MarkerEventAwakeSystem : AwakeSystem<MarkerEventManager>
        {
            protected override void Awake(MarkerEventManager self)
            {
                self.markerDict.Clear();
            }
        }

        public class MarkerEventDestroySystem : DestroySystem<MarkerEventManager>
        {
            protected override void Destroy(MarkerEventManager self)
            {
                self.markerDict.Clear();
            }
        }

        public static void RegistMarkerEvent(this MarkerEventManager self, string markerName, int functionIndex)
        {
            if (self.markerDict.ContainsKey(markerName))
            {
                Log.Error($"already exist MarkerEvent markerName: {markerName}");
                return;
            }

            MarkerEvent markerEvent = self.AddChild<MarkerEvent, string, int>(markerName, functionIndex, true);
            self.markerDict.Add(markerName, markerEvent.Id);
        }

        public static MarkerEvent GetMarkerEvent(this MarkerEventManager self, string markerName)
        {
            if (!self.markerDict.TryGetValue(markerName, out long id))
            {
                Log.Error($"does not exist MarkerEvent markerName: {markerName}");
                return null;
            }

            return self.GetChild<MarkerEvent>(id);
        }

        public static MarkerEvent TryGetMarkerEvent(this MarkerEventManager self, string markerName)
        {
            if (!self.markerDict.TryGetValue(markerName, out long id)) return null;
            return self.GetChild<MarkerEvent>(id);
        }
    }
}