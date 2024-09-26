using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class b2GameManager: Entity, IAwake, IDestroy, IFixedUpdate
    {
        [StaticField]
        public static b2GameManager Instance;

        public b2Game Game;

        public b2World B2World;
        
        public Dictionary<long, b2Body> BodyDict = new();
    }
}