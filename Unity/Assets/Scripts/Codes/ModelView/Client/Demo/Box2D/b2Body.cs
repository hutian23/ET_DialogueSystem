using System;
using System.Collections.Generic;
using Box2DSharp.Dynamics;
using ET.Event;

namespace ET.Client
{
    // 管理物理层的刚体
    [ChildOf(typeof (b2WorldManager))]
    public class b2Body: Entity, IAwake, IDestroy, IPostStep, IPreStep, IFrameLateUpdate
    {
        // note: 刚体会在PreStep生命周期删除
        public Body body;
        public long unitId;
        
        public List<Fixture> Fixtures = new();
        public Dictionary<string, Fixture> FixtureDict = new();
        public Dictionary<string, long> b2BoxDict = new();
        
        public FlipState flip = FlipState.Left;
        public float angle;
        public float velocityX;
        public float velocityY;
        public int hertz = 60;

        // B2World.Step()期间收集碰撞信息，PostStep中执行事件
        public Queue<CollisionInfo> triggerEnterBuffers = new();
        public Queue<CollisionInfo> triggerStayBuffers = new(); 
        public Queue<CollisionInfo> triggerExitBuffers = new();
        
        public Queue<CollisionInfo> collisionEnterBuffers = new();
        public Queue<CollisionInfo> collisionStayBuffers = new();
        public Queue<CollisionInfo> collisionExitBuffers = new();
    }

    [Flags]
    public enum FlipState
    {
        Left = 1,
        Right = -1
    }
}