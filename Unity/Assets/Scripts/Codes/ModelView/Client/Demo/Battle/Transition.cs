using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class Transition : Entity, IAwake, IDestroy
    {
        public HashSet<string> Flags = new();
        public HashSet<string> cachedFlags = new();
    }
}