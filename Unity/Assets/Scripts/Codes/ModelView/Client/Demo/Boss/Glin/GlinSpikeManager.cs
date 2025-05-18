using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class GlinSpikeManager : Entity, IAwake, IDestroy
    {
        public float offset;
        public int waitFrame;
        public int spawnCount;
        public HashSet<int> spawnIndexSet = new();
        
        public ETCancellationToken token;
    }
}