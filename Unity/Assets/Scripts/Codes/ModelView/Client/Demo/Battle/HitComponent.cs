using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class HitComponent : Entity, IAwake, IDestroy
    {
        public long timer;
        public int startIndex;
        public int endIndex;
        public string checkType;
        
        //缓存已经触发过受击的unit的instanceId
        public HashSet<long> buffSet = new ();
    }
}