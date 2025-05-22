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
        
        public string fixtureName;
        public LayerType layerType;
        public bool isTrigger;
        public HitboxType hitboxType;
        public TagType tagType;
        public Vector2 center;
        public Vector2 size;
    }
}
