using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionEnterType.HandleCallback)]
    [FriendOf(typeof(b2Body))]
    public class HandleCollisionEnterCallback : AInvokeHandler<CollisionEnterCallback>
    {
        public override void Handle(CollisionEnterCallback args)
        {
            // b2Body b2Body = Root.Instance.Get(args.info.dataA.InstanceId) as b2Body;
            // b2Body.collisionEnterBuffers.Enqueue(args.info);
            
            b2Box b2Box = Root.Instance.Get(args.buffer.instanceIdA) as b2Box;
            b2Body b2Body = b2Box.GetParent<b2Body>();
            b2Body.collisionEnterBuffers.Enqueue(args.buffer);
        }
    }
}