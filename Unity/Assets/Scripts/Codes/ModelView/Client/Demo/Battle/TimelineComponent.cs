using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class TimelineComponent: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<string, long> markerEventDict = new();
    }
    
    public struct BeforeBehaviorReload
    {
        public long instanceId;
        public int behaviorOrder;
    }
}