using System.Numerics;
using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Client
{
    [ChildOf(typeof(b2Body))]
    public class b2Box : Entity, IAwake<FixtureDef>, IDestroy
    {
        public Fixture fixture;
        public FixtureDef def;
        
        public LayerType LayerType;
        public TagType TagType;
        
        //触发器
        public bool IsTrigger;
        
        //TODO 这里默认夹具形状为Box，以后需要添加其他形状
        public string Name;
        public HitboxType HitboxType;
        public Vector2 center;
        public Vector2 size;
        
        //碰撞事件回调
        public int TriggerEnterId;
        public int TriggerStayId;
        public int TriggerExitId;
        public int CollisionEnterId;
        public int CollisionStayId;
        public int CollisionExitId;
    }
}
