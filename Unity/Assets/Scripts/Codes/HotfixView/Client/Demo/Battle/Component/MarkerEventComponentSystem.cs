using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(MarkerEventComponent))]
    public static class MarkerEventComponentSystem
    {
        public class MarkerEventAwakeSystem : AwakeSystem<MarkerEventComponent>
        {
            protected override void Awake(MarkerEventComponent self)
            {
                self.markerDict.Clear();
            }
        }

        public class MarkerEventDestroySystem : DestroySystem<MarkerEventComponent>
        {
            protected override void Destroy(MarkerEventComponent self)
            {
                self.markerDict.Clear();
            }
        }

        public static void RegistMarkerEvent(this MarkerEventComponent self, MarkerEvent _event)
        {
            if (self.markerDict.TryAdd(_event.markerName, _event))
            {
                return;
            }
            Log.Error($"already exist marker event: {_event.markerName}");
        }

        public static MarkerEvent GetMarkerEvent(this MarkerEventComponent self, string markerName)
        {
            return self.markerDict.GetValueOrDefault(markerName);
        }
    }
}