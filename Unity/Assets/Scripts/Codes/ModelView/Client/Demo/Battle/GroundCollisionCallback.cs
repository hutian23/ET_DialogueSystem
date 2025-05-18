using ET.Event;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class GroundCollisionCallback : Entity, IAwake, IDestroy, IPostStep
    {
        public CollisionInfo info;
        public int functionIndex;
    }
}