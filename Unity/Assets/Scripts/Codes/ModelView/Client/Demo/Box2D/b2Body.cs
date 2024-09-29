using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Dynamics;
using Transform = Box2DSharp.Common.Transform;

namespace ET.Client
{
    [ChildOf(typeof (b2GameManager))]
    public class b2Body: Entity, IAwake, IDestroy
    {
        //记录unit的instanceId
        public long unitId;
        public Body body;

        public Vector2 velocity;
        public int frame;
        public Vector2 totalPos;
        
        //当前步长，b2World中刚体的位置转换信息
        public Transform trans;
        
        //当前帧建立的hitbox
        public List<Fixture> fixtures = new();
    }
}