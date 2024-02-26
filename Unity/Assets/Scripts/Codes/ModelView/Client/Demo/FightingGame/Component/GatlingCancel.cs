using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class GatlingCancel: Entity, IAwake, IDestroy
    {
        public HashSet<string> cancelTags = new();
    }
}