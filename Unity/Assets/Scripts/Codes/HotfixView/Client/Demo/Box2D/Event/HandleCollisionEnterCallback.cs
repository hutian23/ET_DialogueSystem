using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionEnterType.HandleCallback)]
    [FriendOf(typeof(b2Body))]
    public class HandleCollisionEnterCallback : AInvokeHandler<CollisionEnterCallback>
    {
        public override void Handle(CollisionEnterCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.info.dataA.InstanceId) as b2Body;
            b2Body.collisionEnterBuffers.Enqueue(args.info);
        }
    }
}