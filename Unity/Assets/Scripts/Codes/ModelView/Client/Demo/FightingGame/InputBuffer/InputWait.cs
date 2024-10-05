using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(TimelineComponent))]
    public class InputWait: Entity, IAwake, IDestroy
    {
        public ETCancellationToken Token;

        //
        public HashSet<string> InputHandlers = new();
        
        public List<InputCallback> tcss = new();
    }
}