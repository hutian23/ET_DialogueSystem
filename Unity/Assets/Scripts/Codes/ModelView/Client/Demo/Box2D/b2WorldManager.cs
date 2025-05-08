using System.Collections.Generic;
using Box2DSharp.Dynamics;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class b2WorldManager: Entity, IAwake, IDestroy, IFixedUpdate, IPostStep, ILoad, IPreStep, IGizmosUpdate
    {
        [StaticField]
        public static b2WorldManager Instance;
        
        public b2Game Game;
        public b2World B2World;
        public Dictionary<long, long> BodyDict = new(); // Unit和B2Body的映射关系
        public Queue<Body> DisposeQueue = new(); // 刚体的销毁只能在PreStep生命周期中执行
        
        //物理模拟相关的生命周期函数
        public long PreStepTimer;
        public long PostStepTimer;
        public long GizmosTimer;
    }
}