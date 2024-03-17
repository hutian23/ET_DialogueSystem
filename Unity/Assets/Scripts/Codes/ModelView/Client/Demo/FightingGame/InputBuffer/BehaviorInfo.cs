using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof (BehaviorBufferComponent))]
    public class BehaviorInfo: Entity, IAwake, IDestroy
    {
        public uint targetID;
        
        public uint skillType;
        public uint order;
        public string tag;
        
        public List<string> triggers = new();
    }
}