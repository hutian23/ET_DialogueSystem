using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerStayType.TriggerEvent)]
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(b2Body))]
    public class TriggerStay_HandleTriggerEvent : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            
            //Trigger缓冲区缓冲碰撞信息
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            b2Unit.TriggerBuffer.Enqueue(info);
        }
    }
}