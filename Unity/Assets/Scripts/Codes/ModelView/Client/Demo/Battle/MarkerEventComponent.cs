using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class MarkerEventComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<string, MarkerEvent> markerDict = new();
    }

    public struct MarkerEvent
    {
        public string markerName;
        public int startIndex;
        public int endIndex;
    }
}