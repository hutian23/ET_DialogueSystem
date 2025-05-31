using System;
using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Dynamics;
using ET.Event;

namespace ET.Client
{
    [ChildOf(typeof (b2WorldManager))]
    public class b2Body: Entity, IAwake, IDestroy, IPostStep, IPreStep, IFrameLateUpdate
    {
        public Body body;
        public long unitId;

        public HashSet<int> filterSet = new();
        public Dictionary<string, long> b2BoxDict = new();
        
        public FlipState flip = FlipState.Left;
        public float angle;
        public int hertz = 60;
        public Vector2 velocity;

        // B2World.Step()期间收集碰撞信息，PostStep中执行事件
        public Queue<CollisionBuffer> triggerEnterBuffers = new();
        public Queue<CollisionBuffer> triggerStayBuffers = new();
        public Queue<CollisionBuffer> triggerExitBuffers = new();
        public Queue<CollisionBuffer> collisionEnterBuffers = new();
        public Queue<CollisionBuffer> collisionStayBuffers = new();
        public Queue<CollisionBuffer> collisionExitBuffers = new();
    }

    [Flags]
    public enum FlipState
    {
        Left = 1,
        Right = -1
    }
}