using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class MarkerEventManager : Entity, IAwake, IDestroy
    {
        public Dictionary<string, long> markerDict = new();
    }
}