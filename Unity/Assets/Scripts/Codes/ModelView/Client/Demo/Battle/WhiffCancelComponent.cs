using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class WhiffCancelComponent : Entity, IAwake, IDestroy
    {
        public long timer;
        public HashSet<string> Options = new();
    }
}