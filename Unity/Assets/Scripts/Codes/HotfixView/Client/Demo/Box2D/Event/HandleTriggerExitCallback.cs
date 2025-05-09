using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerExitType.HandleCallback)]
    [FriendOf(typeof(b2Body))]
    public class HandleTriggerExitCallback : AInvokeHandler<TriggerExitCallback>
    {
        public override void Handle(TriggerExitCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.info.dataA.InstanceId) as b2Body;
            b2Body.TriggerExitBuffer.Enqueue(args.info);
        }
    }
}