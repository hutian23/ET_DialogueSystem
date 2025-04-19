using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class BlackBoard : Entity, IAwake, IDestroy
    {
        public Dictionary<string, BBValue> ValueDict = new();
    }
}