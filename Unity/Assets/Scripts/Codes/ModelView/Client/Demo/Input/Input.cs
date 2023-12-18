using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class Input : Entity,IAwake,IDestroy, IUpdate
    {
        [StaticField]
        public static Input Instance;
        
        public HashSet<int> Operas = new();
    }
}