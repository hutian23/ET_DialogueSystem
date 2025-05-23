using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionStayType.HandleCallback)]
    [FriendOf(typeof(b2Body))]
    public class HandleCollisionStayCallback : AInvokeHandler<CollisionStayCallback>
    {
        public override void Handle(CollisionStayCallback args)
        {
            // b2Body b2Body = Root.Instance.Get(args.info.dataA.InstanceId) as b2Body;
            // b2Body.collisionStayBuffers.Enqueue(args.info);
            
            b2Box b2Box = Root.Instance.Get(args.buffer.instanceIdA) as b2Box;
            b2Body b2Body = b2Box.GetParent<b2Body>();
            b2Body.collisionStayBuffers.Enqueue(args.buffer);
        }
    }
}