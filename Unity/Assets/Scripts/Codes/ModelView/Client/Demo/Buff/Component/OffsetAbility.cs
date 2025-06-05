using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class OffsetAbility : Entity, IAwake, IFrameUpdate, IDestroy
    {
        public Dictionary<string, long> buffDict = new();
    }
}