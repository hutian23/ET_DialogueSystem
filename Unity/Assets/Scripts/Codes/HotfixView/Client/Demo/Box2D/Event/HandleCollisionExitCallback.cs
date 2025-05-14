using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionExitType.HandleCallback)]
    [FriendOf(typeof(b2Body))]
    public class HandleCollisionExitCallback : AInvokeHandler<CollisionExitCallback>
    {
        public override void Handle(CollisionExitCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.info.dataA.InstanceId) as b2Body;
            b2Body.collisionExitBuffer.Enqueue(args.info);
        }
    }
}