using System.Collections.Generic;
using ET.Event;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class HitComponent : Entity, IAwake<int, string>, IDestroy, IPostStep
    {
        public int functionIndex;
        public string checkType;
        
        // 缓存已经触发过攻击的unit的instanceId
        public HashSet<long> buffSet = new ();
        public CollisionBuffer buffer;
    }
}