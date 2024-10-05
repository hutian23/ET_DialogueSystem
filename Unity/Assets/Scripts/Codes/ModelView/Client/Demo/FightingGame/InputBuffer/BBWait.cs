using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf]
    public class BBWait: Entity, IAwake, IDestroy, ILoad
    {
        public ETCancellationToken token;
        public List<InputCallback> tcss = new();
    }
}