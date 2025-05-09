using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class b2WorldManager: Entity, IAwake, IDestroy, IFixedUpdate, IPostStep, ILoad, IPreStep, IGizmosUpdate
    {
        [StaticField]
        public static b2WorldManager Instance;
        
        public b2Game Game;
        public b2World B2World;
        
        //物理模拟相关的生命周期函数
        public long PreStepTimer;
        public long PostStepTimer;
        public long GizmosTimer;

        public Dictionary<long, long> BodyDict = new();
    }
}