using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof (BBInputComponent))]
    public class BehaviorInfo: Entity, IAwake, IDestroy
    {
        public uint skillType;
        public uint order;
        public string tag;
        
        public List<string> triggers = new();
    }
}