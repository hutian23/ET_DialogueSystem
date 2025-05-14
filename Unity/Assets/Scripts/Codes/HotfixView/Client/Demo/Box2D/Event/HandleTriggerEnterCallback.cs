using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerEnterType.HandleCallback)]
    [FriendOf(typeof(b2Body))]
    public class HandleTriggerEnterCallback : AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.info.dataA.InstanceId) as b2Body;
            b2Body.triggerEnterBuffer.Enqueue(args.info);
        }
    }
}