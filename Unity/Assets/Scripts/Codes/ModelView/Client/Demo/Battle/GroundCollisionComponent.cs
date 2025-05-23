using ET.Event;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class GroundCollisionComponent : Entity, IAwake, IDestroy, IPostStep
    {
        public bool GroundCollision;
        public int functionIndex;
        public CollisionBuffer buffer;
    }
}