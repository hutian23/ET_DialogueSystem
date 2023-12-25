using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class CollisionManager : Entity,IAwake,IDestroy, IUpdate, IFixedUpdate
    {
        [StaticField]
        public static CollisionManager Instance;
        
        public Queue<long> Actors = new();
        public Queue<long> Solids = new();
    }
}