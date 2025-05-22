using System.Numerics;
using Box2DSharp.Dynamics;

namespace ET.Client
{
    [ChildOf(typeof(b2Body))]
    public class b2Box : Entity, IAwake, IDestroy, IGizmosUpdate
    {
        public Fixture fixture;
        public FixtureDef fixtureDef;
        
        public LayerType LayerType;
        public TagType TagType;
        
        //触发器
        public bool IsTrigger;
        
        //TODO 这里默认夹具形状为Box，以后需要添加其他形状
        public string Name;
        public HitboxType HitboxType;
        public Vector2 Center;
        public Vector2 Size;
        
        //碰撞事件回调
        public int TriggerEnterId;
        public int TriggerStayId;
        public int TriggerExitId;
        public int CollisionEnterId;
        public int CollisionStayId;
        public int CollisionExitId;
    }
}
