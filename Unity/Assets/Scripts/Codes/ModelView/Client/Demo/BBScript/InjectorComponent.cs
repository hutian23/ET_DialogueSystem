using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class InjectorComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<int, string> OpDict = new();
        public Dictionary<string, DataGroup> GroupDict = new();
        public HashSet<int> GroupPointerSet = new();

        public ETCancellationToken token;
        public Dictionary<long, int> Coroutine_Pointers = new();
    }
}