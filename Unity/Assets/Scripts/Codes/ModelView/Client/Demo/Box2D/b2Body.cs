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
        
        public FlipState Flip = FlipState.Left;
        public float VelocityX;
        public float VelocityY;
        public int Hertz = 60;

        // B2World.Step()期间收集碰撞信息，PostStep中执行事件
        public Queue<CollisionInfo> TriggerEnterBuffer = new();
        public Queue<CollisionInfo> TriggerStayBuffer = new(); 
        public Queue<CollisionInfo> TriggerExitBuffer = new();
        
        public Queue<CollisionInfo> CollisionEnterBuffer = new();
        public Queue<CollisionInfo> CollisionStayBuffer = new();
        public Queue<CollisionInfo> CollisionExitBuffer = new();
    }

    [Flags]
    public enum FlipState
    {
        Left = 1,
        Right = -1
    }
}