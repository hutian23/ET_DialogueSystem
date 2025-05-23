using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerEnterType.HandleCallback)]
    [FriendOf(typeof(b2Body))]
    public class HandleTriggerEnterCallback : AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
            // b2Body b2Body = Root.Instance.Get(args.info.dataA.InstanceId) as b2Body;
            // b2Body.triggerEnterBuffers.Enqueue(args.info);
            b2Box b2Box = Root.Instance.Get(args.buffer.instanceIdA) as b2Box;
            b2Body b2Body = b2Box.GetParent<b2Body>();
            b2Body.triggerEnterBuffers.Enqueue(args.buffer);
        }
    }
}