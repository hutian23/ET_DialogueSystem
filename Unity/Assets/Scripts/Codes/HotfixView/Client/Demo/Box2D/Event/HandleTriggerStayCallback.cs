using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerStayType.HandleCallback)]
    [FriendOf(typeof(b2Body))]
    public class HandleTriggerStayCallback : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.info.dataA.InstanceId) as b2Body;
            b2Body.triggerStayBuffers.Enqueue(args.info);
        }
    }
}