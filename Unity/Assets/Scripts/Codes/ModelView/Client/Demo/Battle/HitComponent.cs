using System.Collections.Generic;
using ET.Event;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class HitComponent : Entity, IAwake, IDestroy, IPostStep
    {
        public int startIndex;
        public int endIndex;
        public string checkType;
        
        // 缓存已经触发过攻击的unit的instanceId
        public HashSet<long> buffSet = new ();
        public CollisionBuffer buffer;
    }
}