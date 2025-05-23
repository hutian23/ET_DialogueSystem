using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerExitType.HandleCallback)]
    [FriendOf(typeof(b2Body))]
    public class HandleTriggerExitCallback : AInvokeHandler<TriggerExitCallback>
    {
        public override void Handle(TriggerExitCallback args)
        {
            // b2Body b2Body = Root.Instance.Get(args.info.dataA.InstanceId) as b2Body;
            // b2Body.triggerExitBuffers.Enqueue(args.info);
            
            b2Box b2Box = Root.Instance.Get(args.buffer.instanceIdA) as b2Box;
            b2Body b2Body = b2Box.GetParent<b2Body>();
            b2Body.triggerExitBuffers.Enqueue(args.buffer);
        }
    }
}