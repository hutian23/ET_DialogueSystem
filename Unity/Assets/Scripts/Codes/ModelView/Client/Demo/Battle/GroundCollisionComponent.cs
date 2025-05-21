using ET.Event;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class GroundCollisionComponent : Entity, IAwake, IDestroy, IPostStep
    {
        public CollisionInfo info;
        public bool GroundCollision;
        public int functionIndex;
    }
}