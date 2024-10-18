using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (BBParser))]
    public class MarkerEventParser: Entity, IAwake, IDestroy
    {
        public Dictionary<string, int> marker_pointers = new();

        public ETCancellationToken Token = new();
    }
}