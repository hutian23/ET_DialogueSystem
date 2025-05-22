using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionStayType.HandleCallback)]
    [FriendOf(typeof(b2Body))]
    public class HandleCollisionStayCallback : AInvokeHandler<CollisionStayCallback>
    {
        public override void Handle(CollisionStayCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.info.dataA.InstanceId) as b2Body;
            b2Body.collisionStayBuffers.Enqueue(args.info);
        }
    }
}